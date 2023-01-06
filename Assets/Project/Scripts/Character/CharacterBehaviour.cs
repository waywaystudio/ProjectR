using System;
using Core;
using UnityEngine;

namespace Character
{
    public class CharacterBehaviour : MonoBehaviour, ICombatExecutor, IDynamicStatEntry, IEditorSetUp
    {
        [SerializeField] protected string characterName = string.Empty;
        [SerializeField] protected IDCode id;
        [SerializeField] private AliveValue isAlive;
        [SerializeField] private HpValue hp;
        [SerializeField] private ResourceValue resource;
        [SerializeField] private ShieldValue shield;

        public string Name => characterName ??= "Diablo";
        public IDCode ID => id;
        public IDCode ActionCode => IDCode.None;
        public IDynamicStatEntry DynamicStatEntry => this; 
        public ICombatProvider Provider => this;
        public GameObject Object => gameObject;
        public StatTable StatTable { get; } = new();
        public AliveValue IsAlive => isAlive;
        public HpValue Hp => hp;
        public ResourceValue Resource => resource;
        public ShieldValue Shield => shield;

        public ActionTable OnUpdate { get; } = new();
        public ActionTable OnIdle { get; } = new();
        public ActionTable<Vector3, Action> OnWalk { get; } = new();
        public ActionTable<Vector3, Action> OnRun { get; } = new();
        public ActionTable<Vector3> OnTeleport { get; } = new();
        public ActionTable<string, Action> OnSkill { get; } = new(8);
        public ActionTable OnSkillHit { get; } = new(4);
        public ActionTable<ICombatEntity> OnTakeStatusEffect { get; } = new();
        public ActionTable<IDCode> OnDispelStatusEffect { get; } = new();
        public ActionTable<CombatLog> OnCombatActive { get; } = new();
        public ActionTable<CombatLog> OnCombatPassive { get; } = new();
        
        public FunctionTable<bool> IsReached { get; } = new();
        public FunctionTable<Vector3> Direction { get; } = new();

        public ICombatBehaviour CombatBehaviour { get; set; }
        public ISkillInfo SkillInfo { get; set; }
        public ISearching SearchingEngine { get; set; }
        public ITargeting TargetingEngine { get; set; }
        // public IPathfinding PathfindingEngine { get;set; }
        

        public void Idle() => OnIdle?.Invoke();
        public void Walk(Vector3 destination, Action pathCallback = null) => OnWalk?.Invoke(destination, pathCallback);
        public void Run(Vector3 destination, Action pathCallback = null) => OnRun?.Invoke(destination, pathCallback);
        public void Teleport(Vector3 destination) => OnTeleport?.Invoke(destination);
        public void Skill(string skillName, Action animationCallback) => OnSkill?.Invoke(skillName, animationCallback);
        public void SkillHit() => OnSkillHit?.Invoke();

        public void TakeDamage(ICombatEntity provider) => CombatUtility.TakeDamage(provider, this);
        public void TakeSpell(ICombatEntity provider) => CombatUtility.TakeSpell(provider, this);
        public void TakeHeal(ICombatEntity provider) => CombatUtility.TakeHeal(provider, this);
        public void TakeStatusEffect(ICombatEntity statusEffect) => OnTakeStatusEffect?.Invoke(statusEffect);
        public void DispelStatusEffect(IDCode code) => OnDispelStatusEffect?.Invoke(code);
        

        protected void Awake()
        {
            // TODO. 추후에 클래스가 커지면, IDynamicStatEntity를 묶어서 빼도 된다.
            hp.StatTable       = StatTable;
            resource.StatTable = StatTable;
            shield.StatTable   = StatTable;
        }

        protected virtual void Start()
        {
            IsAlive.Value  = true;
            Hp.Value       = StatTable.MaxHp;
            Resource.Value = StatTable.MaxResource;
            Shield.Value   = 0;
        }

        protected virtual void Update()
        {
            OnUpdate?.Invoke();
        }
#if UNITY_EDITOR
        public virtual void SetUp()
        {
            if (characterName == string.Empty)
            {
                Debug.LogError("CharacterName Required");
            }
        }
        // private void ShowLog(CombatLog log)
        // {
        //     Debug.Log($"Combat : IsHit:{log.IsHit} IsCritical:{log.IsCritical} Value:{log.Value} " +
        //               $"Provider:{log.Provider} Taker:{log.Taker} Skill:{log.ActionName}");
        // }
#endif
    }
}
