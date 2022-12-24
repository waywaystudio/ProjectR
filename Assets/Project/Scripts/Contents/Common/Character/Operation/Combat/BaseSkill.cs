using System.Collections.Generic;
using System.Linq;
using Common.Character.Operation.Combat.Entity;
using Core;
using UnityEngine;
// ReSharper disable MemberCanBeProtected.Global

namespace Common.Character.Operation.Combat
{
    public abstract class BaseSkill : MonoBehaviour, IActionSender
    {
        [SerializeField] protected int id;
        [SerializeField] protected string actionName;
        [SerializeField] protected int priority;
        
        protected int InstanceID;
        protected CharacterBehaviour Cb;
        protected Combating Combat;

        public int ID => id;
        public int Priority => priority;
        public string ActionName => actionName;
        public ICombatProvider Sender => Cb;

        public Dictionary<EntityType, BaseEntity> EntityTable { get; } = new();
        
        public bool IsSkillFinished { get; set; }
        public bool IsSkillReady => EntityTable.All(x => x.Value.IsReady);
        public bool IsCoolTimeReady =>!EntityTable.ContainsKey(EntityType.CoolTime) || 
                                       EntityTable[EntityType.CoolTime].IsReady;

        public virtual void StartSkill()
        {
            IsSkillFinished = false;
            EntityTable.ForEach(x => x.Value.OnStarted?.Invoke());
        }
        public virtual void CompleteSkill()
        {
            EntityTable.ForEach(x => x.Value.OnCompleted?.Invoke());
            IsSkillFinished = true;
        } 
        public void InterruptedSkill() 
            => EntityTable.ForEach(x => x.Value.OnInterrupted?.Invoke());

        /// <summary>
        /// Register Animation Event.
        /// </summary>
        public virtual void InvokeEvent(){}
        public virtual void ActiveSkill()
        {
            Cb.OnSkill.Register(InstanceID, StartSkill);
            Cb.OnSkillHit.Register(InstanceID, InvokeEvent);
            Cb.Skill(ActionName, CompleteSkill);
        }

        public void DeActiveSkill()
        {
            Cb.OnSkill.Unregister(InstanceID);
            Cb.OnSkillHit.Unregister(InstanceID);
        }
        
        public bool TryGetEntity<T>(EntityType entityType, out T result) where T : BaseEntity
        {
            if (!EntityTable.TryGetValue(entityType, out var entity))
            {
                result = null;
                return false;
            }
            
            result = entity as T;
            return true;
        }

        protected DamageEntity DamageEntity => EntityTable[EntityType.Damage] as DamageEntity;
        protected CastingEntity CastingEntity => EntityTable[EntityType.Casting] as CastingEntity;
        protected CoolTimeEntity CoolTimeEntity => EntityTable[EntityType.CoolTime] as CoolTimeEntity;
        protected HealEntity HealEntity => EntityTable[EntityType.Heal] as HealEntity;
        protected ProjectileEntity ProjectileEntity => EntityTable[EntityType.Projectile] as ProjectileEntity;
        protected StatusEffectEntity StatusEffectEntity => EntityTable[EntityType.StatusEffect] as StatusEffectEntity;
        protected TargetEntity TargetEntity => EntityTable[EntityType.Target] as TargetEntity;

        protected void Awake()
        {
            InstanceID = GetInstanceID();
            Cb = GetComponentInParent<CharacterBehaviour>();
            Combat = GetComponentInParent<Combating>();
            actionName ??= GetType().Name;
            
            GetComponents<BaseEntity>().ForEach(x => EntityTable.Add(x.Flag, x));
            EntityTable.ForEach(x => x.Value.Initialize(this));
        }

        protected void OnEnable()
        {
            Combat.SkillTable.TryAdd(id, this);
        }

        protected void OnDisable()
        {
            Combat.SkillTable.TryRemove(id);
            DeActiveSkill();
        }

        protected virtual void Reset() => actionName = GetType().Name;

#if UNITY_EDITOR
        #region EditorOnly
        protected void GetDataFromDB()
        {
            var skillData = MainGame.MainData.GetSkillData(actionName);
            
            actionName.IsNullOrEmpty().OnTrue(() => actionName = GetType().Name);
            id = skillData.ID;
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
            
            UnityEditor.EditorUtility.SetDirty(this);
        }
        protected void ShowDB()
        {
            UnityEditor.EditorUtility.OpenPropertyEditor
                (MainGame.MainData.DataObjectList.Find(x => x.Category == MainGame.Data.DataCategory.Skill));
        }
        #endregion
#endif
    }
}
