using System.Collections.Generic;
using System.Linq;
using Character.Combat.Entities;
using Core;
using UnityEngine;
// ReSharper disable MemberCanBeProtected.Global

namespace Character.Combat.Skill
{
    public abstract class BaseSkill : MonoBehaviour, IActionSender, IEditorSetUp
    {
        [SerializeField] protected IDCode actionCode;
        [SerializeField] protected int priority;
        [SerializeField] protected Sprite icon;

        protected int InstanceID;
        protected CharacterBehaviour Cb;

        public IDCode ActionCode => actionCode;
        public ICombatProvider Provider => Cb;
        public int Priority => priority;
        public Sprite Icon => icon;
        
        public bool IsSkillFinished { get; set; }
        public bool IsSkillReady => EntityTable.All(x => x.Value.IsReady);
        public bool IsCoolTimeReady => CoolTimeEntity is null || CoolTimeEntity.IsReady;
        
        public DamageEntity DamageEntity => GetEntity<DamageEntity>(EntityType.Damage);
        public CastingEntity CastingEntity => GetEntity<CastingEntity>(EntityType.Casting);
        public CoolTimeEntity CoolTimeEntity => GetEntity<CoolTimeEntity>(EntityType.CoolTime);
        public HealEntity HealEntity => GetEntity<HealEntity>(EntityType.Heal);
        public ProjectileEntity ProjectileEntity => GetEntity<ProjectileEntity>(EntityType.Projectile);
        public StatusEffectEntity StatusEffectEntity => GetEntity<StatusEffectEntity>(EntityType.StatusEffect);
        public TargetEntity TargetEntity => GetEntity<TargetEntity>(EntityType.Target);
        public ResourceEntity ResourceEntity => GetEntity<ResourceEntity>(EntityType.Resource);
        
        private ActionTable OnStart { get; } = new();
        private ActionTable OnComplete { get; } = new();
        private ActionTable OnInterrupted { get; } = new();
        private Dictionary<EntityType, BaseEntity> EntityTable { get; } = new();


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
        

        protected virtual void StartSkill() => OnStart.Invoke();
        protected virtual void CompleteSkill() => OnComplete.Invoke();
        protected void InterruptedSkill() => OnInterrupted.Invoke();

        private T GetEntity<T>(EntityType type) where T : BaseEntity 
                => EntityTable.ContainsKey(type) 
                ? EntityTable[type] as T 
                : null;

        protected void Awake()
        {
            InstanceID = GetInstanceID();
            Cb = GetComponentInParent<CharacterBehaviour>();
            
            if (actionCode == IDCode.None) 
                actionCode = GetType().Name.ToEnum<IDCode>();
            
            GetComponents<BaseEntity>().ForEach(x => EntityTable.Add(x.Flag, x));
            EntityTable.ForEach(x =>
            {
                x.Value.Initialize(this);
                
                OnStart.Register(x.Value.InstanceID, x.Value.OnStarted.Invoke);
                OnComplete.Register(x.Value.InstanceID, x.Value.OnCompleted.Invoke);
                OnInterrupted.Register(x.Value.InstanceID, x.Value.OnInterrupted.Invoke);
            });
            
            OnStart.Register(InstanceID, () => IsSkillFinished = false);
            OnComplete.Register(InstanceID, () => IsSkillFinished = true);
        }

        protected void OnDisable() => DeActiveSkill();
        protected virtual void Reset() => actionCode = GetType().Name.ToEnum<IDCode>();
        

#if UNITY_EDITOR
        public void SetUp()
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
#endif
    }
}
