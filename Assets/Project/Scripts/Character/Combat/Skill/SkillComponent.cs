using System.Collections.Generic;
using Character.Graphic;
using Core;
using UnityEngine;

namespace Character.Combat.Skill
{
    public class SkillComponent : MonoBehaviour, IActionSequence
    {
        [SerializeField] private DataIndex id;
        // [SerializeField] private SkillData data;
        [SerializeField] private AnimationModel animationModel;
        [SerializeField] private ModuleController moduleController;

        protected ICombatProvider Provider { get; set; }

        public ActionTable OnActivated { get; } = new();
        public ActionTable OnInterrupted { get; } = new();
        public ActionTable OnHit { get; } = new();
        public ActionTable OnCompleted { get; } = new();

        // 1. Casting Type 분별
        // 1-1. Casting Progress
        // 2. CoolTime Type 분별
        // 2-1 CoolTime Progress
        // 3. Condition에서 어떤 Module의 False분별
        

        public void UseSkill()
        {
            if (!moduleController.IsReadyToActive()) return;

            OnActivated.Invoke();
        }


        private void Awake()
        {
            Provider = GetComponentInParent<ICombatProvider>();
            moduleController.Initialize(Provider, this);
        }

        private void OnEnable()
        {
            OnActivated.Register("PlayAnimation", PlayAnimation);
            // animationModel.Play(Data.AnimationKey, 0, false, Data.TimeScale, Idle);
        }

        private void PlayAnimation()
        {
            // animationModel.OnHit.Register(GetInstanceID(), OnHit.Invoke());
            // animationModel.Play(Data.AnimationKey, 0, false, Data.TimeScale, Idle);
        }

        // protected void SetUp();
        // private void ShowDB();
    }

    public interface IActionSequence
    {
        ActionTable OnActivated { get; }
        ActionTable OnInterrupted { get; }
        ActionTable OnHit { get; }
        ActionTable OnCompleted { get; }
    }

    public class ModuleController : MonoBehaviour
    {
        [SerializeField] private List<CombatModule> modulesList;

        public Dictionary<ModuleType, CombatModule> ModuleTable { get; } = new();

        public DamageModule DamageModule => Get<DamageModule>(ModuleType.Damage);
        public CastingModule CastingModule => Get<CastingModule>(ModuleType.Casting);
        public CoolTimeModule CoolTimeModule => Get<CoolTimeModule>(ModuleType.CoolTime);
        public HealModule HealModule => Get<HealModule>(ModuleType.Heal);
        public ProjectileModule ProjectileModule => Get<ProjectileModule>(ModuleType.Projectile);
        public StatusEffectModule StatusEffectModule => Get<StatusEffectModule>(ModuleType.StatusEffect);
        public TargetModule TargetModule => Get<TargetModule>(ModuleType.Target);
        public ResourceModule ResourceModule => Get<ResourceModule>(ModuleType.Resource);
        public ProjectorModule ProjectorModule => Get<ProjectorModule>(ModuleType.Projector);

        public void Initialize(ICombatProvider provider, IActionSequence actionSequence)
        {
            if (ModuleTable.IsNullOrEmpty()) modulesList.ForEach(x => ModuleTable.TryAdd(x.Flag, x));

            ModuleTable.ForEach(x => x.Value.Initialize(provider, actionSequence));
        }

        public bool IsReadyToActive()
        {
            if (CastingModule && !CastingModule.IsReady)
            {
                Debug.LogWarning("Casting is On");
                return false;
            }

            if (CoolTimeModule && !CoolTimeModule.IsReady)
            {
                Debug.LogWarning("CoolTime is Not Ready");
                return false;
            }
            
            return true;
        }
        
        // public void AddModule(ModuleType type)
        // public void RemoveModule(ModuleType type)

        private T Get<T>(ModuleType type) where T : CombatModule =>
            ModuleTable.ContainsKey(type)
                ? ModuleTable[type] as T
                : null;
    }

    // [Serializable]
    // public class SkillData
    // {
    //     [SerializeField] private DataIndex actionCode;
    //     [SerializeField] private string skillName;
    //     [SerializeField] private float power;
    //     [SerializeField] private float coolTime;
    //     [SerializeField] private float castingTime; 
    //     [SerializeField] private float range;
    //     [SerializeField] private int priority;
    //     [SerializeField] private string animationKey;
    //     [SerializeField] private Sprite Icon;
    //     [SerializeField] private string description;
    //     
    //     public DataIndex ActionCode => actionCode;
    //     public string SkillName => skillName;
    //     public float Power => power;
    //     public float CoolTime => coolTime;
    //     public float CastingTime => castingTime;
    //     public float Range => range;
    //     public int Priority => priority;
    //     public string AnimationKey => animationKey;
    //
    //     public void Init(DataIndex actionCode)
    //     {
    //         var skillData = MainGame.MainData.GetSkill(actionCode);
    //
    //         this.actionCode = actionCode;
    //         skillName       = skillData.Name;
    //         power           = skillData.BaseValue;
    //         coolTime        = skillData.BaseCoolTime;
    //         castingTime     = skillData.CastingTime;
    //         range           = skillData.Range;
    //         priority        = skillData.Priority;
    //         animationKey    = skillData.AnimationKey;
    //     }
    // }
}

