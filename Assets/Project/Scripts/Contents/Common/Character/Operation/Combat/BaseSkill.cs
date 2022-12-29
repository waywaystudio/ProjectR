using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;
// ReSharper disable MemberCanBeProtected.Global

namespace Common.Character.Operation.Combat
{
    using Entity;
    
    public abstract class BaseSkill : MonoBehaviour, IActionSender
    {
        [SerializeField] protected IDCode id;
        [SerializeField] protected string actionName;
        [SerializeField] protected int priority;
        [SerializeField] protected Sprite icon;

        protected int InstanceID;
        protected CharacterBehaviour Cb;
        protected Combating Combat;

        public IDCode ID => id;
        public int Priority => priority;
        public string ActionName => actionName;
        public ICombatProvider Sender => Cb;
        public Sprite Icon => icon;

        public ActionTable OnStart { get; } = new();
        public ActionTable OnComplete { get; } = new();
        public ActionTable OnInterrupted { get; } = new();
        public Dictionary<EntityType, BaseEntity> EntityTable { get; } = new();
        
        public bool IsSkillFinished { get; set; }
        public bool IsSkillReady => EntityTable.All(x => x.Value.IsReady);
        public bool IsCoolTimeReady => CoolTimeEntity is null || CoolTimeEntity.IsReady;

        public virtual void StartSkill() => OnStart.Invoke();
        public virtual void CompleteSkill() => OnComplete.Invoke();
        public void InterruptedSkill() => OnInterrupted.Invoke();

        /// <summary>
        /// Register Animation Event.
        /// </summary>
        public virtual void InvokeEvent(){}
        public virtual void ActiveSkill()
        {
            Cb.CurrentSkill = this;
            Cb.OnSkill.Register(InstanceID, StartSkill);
            Cb.OnSkillHit.Register(InstanceID, InvokeEvent);
            Cb.Skill(ActionName, CompleteSkill);
        }

        public void DeActiveSkill()
        {
            Cb.CurrentSkill = null;
            Cb.OnSkill.Unregister(InstanceID);
            Cb.OnSkillHit.Unregister(InstanceID);
        }


        protected DamageEntity DamageEntity 
            => EntityTable.ContainsKey(EntityType.Damage) 
                ? EntityTable[EntityType.Damage] as DamageEntity : null;
        
        public CastingEntity CastingEntity
            => EntityTable.ContainsKey(EntityType.Casting) 
                ? EntityTable[EntityType.Casting] as CastingEntity : null;

        public CoolTimeEntity CoolTimeEntity
            => EntityTable.ContainsKey(EntityType.CoolTime) 
                ? EntityTable[EntityType.CoolTime] as CoolTimeEntity : null;

        protected HealEntity HealEntity
            => EntityTable.ContainsKey(EntityType.Heal) 
                ? EntityTable[EntityType.Heal] as HealEntity : null;

        protected ProjectileEntity ProjectileEntity
            => EntityTable.ContainsKey(EntityType.Projectile)
                ? EntityTable[EntityType.Projectile] as ProjectileEntity: null;
        
        protected StatusEffectEntity StatusEffectEntity 
            => EntityTable.ContainsKey(EntityType.StatusEffect)
                ? EntityTable[EntityType.StatusEffect] as StatusEffectEntity : null;

        public TargetEntity TargetEntity
            => EntityTable.ContainsKey(EntityType.Target)
                ? EntityTable[EntityType.Target] as TargetEntity : null;
        
        public ResourceEntity ResourceEntity
            => EntityTable.ContainsKey(EntityType.Resource)
                ? EntityTable[EntityType.Resource] as ResourceEntity : null;

        protected void Awake()
        {
            InstanceID = GetInstanceID();
            Cb = GetComponentInParent<CharacterBehaviour>();
            Combat = GetComponentInParent<Combating>();
            actionName ??= GetType().Name;
            
            GetComponents<BaseEntity>().ForEach(x => EntityTable.Add(x.Flag, x));
            EntityTable.ForEach(x =>
            {
                x.Value.Initialize(this);
                
                OnStart.Register(x.Value.GetInstanceID(), x.Value.OnStarted.Invoke);
                OnComplete.Register(x.Value.GetInstanceID(), x.Value.OnCompleted.Invoke);
                OnInterrupted.Register(x.Value.GetInstanceID(), x.Value.OnInterrupted.Invoke);
            });
            
            OnStart.Register(InstanceID, () => IsSkillFinished = false);
            OnComplete.Register(InstanceID, () => IsSkillFinished = true);
        }

        protected void OnEnable()
        {
            Combat.SkillTable.TryAdd((int)id, this);
        }

        protected void OnDisable()
        {
            Combat.SkillTable.TryRemove((int)id);
            DeActiveSkill();
        }

        protected virtual void Reset() => actionName = GetType().Name;

#if UNITY_EDITOR
        #region EditorOnly
        protected void GetDataFromDB()
        {
            var skillData = MainGame.MainData.GetSkill(actionName.ToEnum<IDCode>());
            
            actionName.IsNullOrEmpty().OnTrue(() => actionName = GetType().Name);
            id = (IDCode)skillData.ID;
            priority = skillData.Priority;

            if (TryGetComponent(out DamageEntity damageEntity))
            {
                damageEntity.DamageValue = skillData.BaseValue;
                damageEntity.Flag = EntityType.Damage;
            }
            if (TryGetComponent(out CastingEntity castingEntity))
            {
                castingEntity.OriginalCastingTime = skillData.CastingTime;
                castingEntity.Flag = EntityType.Casting;
            }
            if (TryGetComponent(out CoolTimeEntity coolTimeEntity))
            {
                coolTimeEntity.CoolTime = skillData.BaseCoolTime;
                coolTimeEntity.Flag = EntityType.CoolTime;
            }
            if (TryGetComponent(out HealEntity healEntity))
            {
                healEntity.HealValue = skillData.BaseValue;
                healEntity.Flag = EntityType.Heal;
            }
            if (TryGetComponent(out ProjectileEntity projectileEntity))
            {
                projectileEntity.ProjectileName = skillData.Projectile;
                projectileEntity.Flag = EntityType.Projectile;
            }
            if (TryGetComponent(out StatusEffectEntity statusEffectEntity))
            {
                statusEffectEntity.ActionName = skillData.StatusEffect;
                statusEffectEntity.Flag = EntityType.StatusEffect;
            }
            if (TryGetComponent(out TargetEntity targetEntity))
            {
                targetEntity.TargetLayerType = skillData.TargetLayer;
                targetEntity.TargetCount = skillData.TargetCount;
                targetEntity.Range = skillData.Range;
                targetEntity.Flag = EntityType.Target;
            }
            if (TryGetComponent(out ResourceEntity resourceEntity))
            {
                resourceEntity.Obtain = skillData.ResourceObtain;
                resourceEntity.Flag = EntityType.Resource;
            }
            
            UnityEditor.EditorUtility.SetDirty(this);
        }
        protected void ShowDB()
        {
            // UnityEditor.EditorUtility.OpenPropertyEditor
            //     (MainGame.MainData.DataObjectList.Find(x => x.CategoryIndex == 13));
        }
        #endregion
#endif
    }
}
