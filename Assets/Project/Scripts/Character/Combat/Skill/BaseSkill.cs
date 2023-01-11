using System.Collections.Generic;
using Character.Combat.Entities;
using Core;
using UnityEngine;
// ReSharper disable MemberCanBeProtected.Global

namespace Character.Combat.Skill
{
    public abstract class BaseSkill : MonoBehaviour, ISkill, IEditorSetUp
    {
        [SerializeField] protected DataIndex actionCode;
        [SerializeField] protected int priority;
        [SerializeField] protected Sprite icon;

        protected int InstanceID;
        protected CharacterBehaviour Cb;
        
        public DataIndex ActionCode => actionCode;
        public ICombatProvider Provider => Cb;
        public Sprite Icon => icon;
        public bool HasCastingEntity => EntityTable.ContainsKey(EntityType.Casting);
        public float CastingTime => CastingEntity.CastingTime;
        public float CastingProgress => CastingEntity.CastingProgress;
        public bool HasCoolTimeEntity => EntityTable.ContainsKey(EntityType.CoolTime);
        public float CoolTime => CoolTimeEntity.CoolTime;
        public Observable<float> RemainTime => CoolTimeEntity.RemainTime;

        public int Priority => priority;
        public bool IsSkillFinished { get; set; }
        public bool IsCoolTimeReady => CoolTimeEntity is null || CoolTimeEntity.IsReady;

        public bool IsSkillReady 
        {
            get
            {
                // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
                foreach (var item in EntityTable)
                {
                    if (!item.Value.IsReady) 
                        return false;
                }

                return true;
            }
        }

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
            Cb.SkillInfo = this;
            Cb.OnSkill.Register(InstanceID, StartSkill);
            Cb.OnSkillHit.Register(InstanceID, InvokeEvent);
            Cb.Skill(actionCode.ToString(), CompleteSkill);
        }

        public void DeActiveSkill()
        {
            Cb.SkillInfo = null;
            Cb.OnSkill.Unregister(InstanceID);
            Cb.OnSkillHit.Unregister(InstanceID);
        }
        

        protected virtual void StartSkill() => OnStart.Invoke();
        protected virtual void CompleteSkill() => OnComplete.Invoke();
        protected void InterruptedSkill() => OnInterrupted.Invoke();

        private T GetEntity<T>(EntityType type) where T : BaseEntity =>
            EntityTable.ContainsKey(type)
                ? EntityTable[type] as T
                : null;

        protected virtual void Awake()
        {
            InstanceID = GetInstanceID();
            Cb = GetComponentInParent<CharacterBehaviour>();
            
            if (actionCode == DataIndex.None) 
                actionCode = GetType().Name.ToEnum<DataIndex>();
            
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


#if UNITY_EDITOR
        public void SetUp()
        {
            if (actionCode == DataIndex.None) 
                actionCode = GetType().Name.ToEnum<DataIndex>();

            var skillData = MainGame.MainData.GetSkill(actionCode);
            priority = skillData.Priority;

            GetComponents<BaseEntity>().ForEach(x => EntityUtility.SetSkillEntity(skillData, x));
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
        protected void Reset() => actionCode = GetType().Name.ToEnum<DataIndex>();
        
        private void ShowDB()
        {
            UnityEditor.EditorUtility.OpenPropertyEditor
                (MainGame.MainData.DataList.Find(x => x.Index == 13));
        }
#endif
    }
}

/* Annotation
 * 스킬은 Entity의 집합이며, 최소 한개의 ICombatEntity를 가진다. */
