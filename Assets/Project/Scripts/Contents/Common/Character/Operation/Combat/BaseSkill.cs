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
        [SerializeField] protected IDCode actionCode;
        [SerializeField] protected int priority;
        [SerializeField] protected Sprite icon;

        protected int InstanceID;
        protected CharacterBehaviour Cb;
        protected CombatOperation Combat;

        public int Priority => priority;
        public IDCode ActionCode => actionCode;
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
            Cb.OnSkill.Register(InstanceID, StartSkill);
            Cb.OnSkillHit.Register(InstanceID, InvokeEvent);
            Cb.Skill(actionCode.ToString(), CompleteSkill);
        }

        public void DeActiveSkill()
        {
            Cb.OnSkill.Unregister(InstanceID);
            Cb.OnSkillHit.Unregister(InstanceID);
        }

        public DamageEntity DamageEntity => GetEntity<DamageEntity>(EntityType.Damage);
        public CastingEntity CastingEntity => GetEntity<CastingEntity>(EntityType.Casting);
        public CoolTimeEntity CoolTimeEntity => GetEntity<CoolTimeEntity>(EntityType.CoolTime);
        public HealEntity HealEntity => GetEntity<HealEntity>(EntityType.Heal);
        public ProjectileEntity ProjectileEntity => GetEntity<ProjectileEntity>(EntityType.Projectile);
        public StatusEffectEntity StatusEffectEntity => GetEntity<StatusEffectEntity>(EntityType.StatusEffect);
        public TargetEntity TargetEntity => GetEntity<TargetEntity>(EntityType.Target);
        public ResourceEntity ResourceEntity => GetEntity<ResourceEntity>(EntityType.Resource);
        
        private T GetEntity<T>(EntityType type) where T : BaseEntity => EntityTable.ContainsKey(type) 
                ? EntityTable[type] as T 
                : null;

        protected void Awake()
        {
            InstanceID = GetInstanceID();
            Cb = GetComponentInParent<CharacterBehaviour>();
            Combat = GetComponentInParent<CombatOperation>();
            
            if (actionCode == IDCode.None) 
                actionCode = GetType().Name.ToEnum<IDCode>();
            
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
            Combat.SkillTable.TryAdd((int)actionCode, this);
        }

        protected void OnDisable()
        {
            Combat.SkillTable.TryRemove((int)actionCode);
            DeActiveSkill();
        }

        protected virtual void Reset() => actionCode = GetType().Name.ToEnum<IDCode>();
        

#if UNITY_EDITOR
        #region EditorOnly
        private void SetUp()
        {
            if (actionCode == IDCode.None) 
                actionCode = GetType().Name.ToEnum<IDCode>();

            var skillData = MainGame.MainData.GetSkill(actionCode);
            priority = skillData.Priority;

            GetComponents<BaseEntity>().ForEach(x => EntityUtility.SetSkillEntity(skillData, x));
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
        private void ShowDB()
        {
            UnityEditor.EditorUtility.OpenPropertyEditor
                (MainGame.MainData.DataList.Find(x => x.Index == 13));
        }
        #endregion
#endif
    }
}
