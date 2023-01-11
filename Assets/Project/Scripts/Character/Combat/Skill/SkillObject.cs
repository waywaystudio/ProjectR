using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;
// ReSharper disable MemberCanBeProtected.Global

namespace Character.Combat.Skill
{
    using Modules;
    
    public abstract class SkillObject : CombatObject, ISkill, IEditorSetUp
    {
        [SerializeField] protected int priority;
        [SerializeField] protected Sprite icon;

        protected CharacterBehaviour Cb;

        public override ICombatProvider Provider => Cb;
        public Sprite Icon => icon;
        public bool HasCastingEntity => ModuleTable.ContainsKey(ModuleType.Casting);
        public float CastingTime => CastingModule.CastingTime;
        public float CastingProgress => CastingModule.CastingProgress;
        public bool HasCoolTimeEntity => ModuleTable.ContainsKey(ModuleType.CoolTime);
        public float CoolTime => CoolTimeModule.OriginalCoolTime;
        public Observable<float> RemainTime => CoolTimeModule.RemainTime;

        public int Priority => priority;
        public bool IsSkillFinished { get; set; }
        public bool IsCoolTimeReady => CoolTimeModule is null || CoolTimeModule.IsReady;
        public bool IsSkillReady 
        {
            get
            {
                // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
                foreach (var item in ReadyCheckList) if (!item.IsReady) return false;
                
                return true;
            }
        }

        public List<IReady> ReadyCheckList { get; set; } = new();

        public DamageSkill DamageModule => GetModule<DamageSkill>(ModuleType.Damage);
        public CastingSkill CastingModule => GetModule<CastingSkill>(ModuleType.Casting);
        public CoolTimeSkill CoolTimeModule => GetModule<CoolTimeSkill>(ModuleType.CoolTime);
        public HealSkill HealModule => GetModule<HealSkill>(ModuleType.Heal);
        public ProjectileSkill ProjectileModule => GetModule<ProjectileSkill>(ModuleType.Projectile);
        public StatusEffectSkill StatusEffectModule => GetModule<StatusEffectSkill>(ModuleType.StatusEffect);
        public TargetSkill TargetModule => GetModule<TargetSkill>(ModuleType.Target);
        public ResourceSkill ResourceModule => GetModule<ResourceSkill>(ModuleType.Resource);
        
        private ActionTable OnStart { get; } = new();
        private ActionTable OnComplete { get; } = new();
        private ActionTable OnInterrupted { get; } = new();
        
        
        public override void Initialize(ICombatProvider provider, ICombatTaker taker)
        {
            Provider       = provider;
            ReadyCheckList = GetComponents<IReady>().ToList();

            ModuleTable.ForEach(x =>
            {
                x.Value.Initialize(this);
                
                if (x.Value.TryGetComponent(out IOnStarted iOnStarted)) 
                    OnStart.Register(x.Value.InstanceID, iOnStarted.OnStarted.Invoke);
                
                if (x.Value.TryGetComponent(out IOnCompleted iOnCompleted)) 
                    OnComplete.Register(x.Value.InstanceID, iOnCompleted.OnCompleted.Invoke);
                
                if (x.Value.TryGetComponent(out IOnInterrupted iOnInterrupted)) 
                    OnInterrupted.Register(x.Value.InstanceID, iOnInterrupted.OnInterrupted.Invoke);
            });
            
            OnStart.Register(InstanceID, () => IsSkillFinished    = false);
            OnComplete.Register(InstanceID, () => IsSkillFinished = true);
        }

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

        protected override void Awake()
        {
            base.Awake();
            
            Cb = GetComponentInParent<CharacterBehaviour>();
            
            // SkillObject 특수상황. Self Initialize.
            Initialize(Cb, null);
        }

        protected void OnDisable() => DeActiveSkill();


#if UNITY_EDITOR
        public void SetUp()
        {
            if (actionCode == DataIndex.None) 
                actionCode = GetType().Name.ToEnum<DataIndex>();

            var skillData = MainGame.MainData.GetSkill(actionCode);
            priority = skillData.Priority;

            GetComponents<SkillModule>().ForEach(x => EntityUtility.SetSkillEntity(skillData, x));
            UnityEditor.EditorUtility.SetDirty(this);
        }

        private void ShowDB()
        {
            UnityEditor.EditorUtility.OpenPropertyEditor
                (MainGame.MainData.DataList.Find(x => x.Index == 13));
        }
        private void Reset() => actionCode = GetType().Name.ToEnum<DataIndex>();
#endif
    }
}

/* Annotation
 * 스킬은 Entity의 집합이며, 최소 한개의 ICombatEntity를 가진다. */
