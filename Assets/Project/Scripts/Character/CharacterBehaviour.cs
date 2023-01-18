using System;
using Core;
using UnityEngine;
// ReSharper disable UnusedMember.Global

namespace Character
{
    using Combat.Skill;
    
    public class CharacterBehaviour : MonoBehaviour, ICombatExecutor, IInspectorSetUp
    {
        [SerializeField] protected string characterName = string.Empty;
        [SerializeField] protected DataIndex dataIndex;
        [SerializeField] protected RoleType role;

        private ICombatBehaviour combatBehaviour;
        private ISearching searchingEngine;
        private ITargeting targetingEngine;
        private IPathfinding pathfindingEngine;

        public string Name => characterName ??= "Diablo";
        public DataIndex DataIndex => dataIndex;
        public RoleType Role => role;
        public DataIndex ActionCode => DataIndex.None;
        public IDynamicStatEntry DynamicStatEntry { get; set; }
        public ICombatProvider Provider => this;
        public GameObject Object => gameObject;
        public StatTable StatTable { get; } = new(1);

        public ActionTable OnUpdate { get; } = new();
        public ActionTable OnIdle { get; } = new();
        public ActionTable<Vector3, Action> OnWalk { get; } = new();
        public ActionTable<Vector3, Action> OnRun { get; } = new();
        public ActionTable<Vector3> OnTeleport { get; } = new();

        public ActionTable<SkillObject> OnUseSkill { get; } = new(4);
        public ActionTable OnActiveSkill { get; } = new(8);
        public ActionTable OnCompleteSkill { get; } = new(4);
        public ActionTable OnHitSkill { get; } = new(4);
        public ActionTable OnCancelSkill { get; } = new(4);

        public ActionTable<IStatusEffect> OnTakeStatusEffect { get; } = new(2);
        public ActionTable<DataIndex> OnDispelStatusEffect { get; } = new(2);
        public ActionTable<CombatLog> OnCombatActive { get; } = new(4);
        public ActionTable<CombatLog> OnCombatPassive { get; } = new(4);

        public ICombatBehaviour CombatBehaviour => combatBehaviour ??= GetComponentInChildren<ICombatBehaviour>();
        public ISearching SearchingEngine => searchingEngine ??= GetComponentInChildren<ISearching>();
        public ITargeting TargetingEngine => targetingEngine ??= GetComponentInChildren<ITargeting>();
        public IPathfinding PathfindingEngine => pathfindingEngine ??= GetComponentInChildren<IPathfinding>();
        public ISkillInfo SkillInfo { get; set; }

        public void Idle() => OnIdle?.Invoke();
        public void Walk(Vector3 destination, Action pathCallback = null) => OnWalk?.Invoke(destination, pathCallback);
        public void Run(Vector3 destination, Action pathCallback = null) => OnRun?.Invoke(destination, pathCallback);
        public void Teleport(Vector3 destination) => OnTeleport?.Invoke(destination);
        public void UseSkill(SkillObject skill) => OnUseSkill.Invoke(skill);
        public void ActiveSkill() => OnActiveSkill.Invoke();
        public void CompleteSkill() => OnCompleteSkill?.Invoke();
        public void HitSkill() => OnHitSkill?.Invoke();
        public void CancelSkill() => OnCancelSkill?.Invoke();

        public void TakeDamage(ICombatTable provider) => CombatUtility.TakeDamage(provider, this);
        public void TakeSpell(ICombatTable provider) => CombatUtility.TakeSpell(provider, this);
        public void TakeHeal(ICombatTable provider) => CombatUtility.TakeHeal(provider, this);
        public void TakeStatusEffect(IStatusEffect statusEffect) => OnTakeStatusEffect?.Invoke(statusEffect);
        public void DispelStatusEffect(DataIndex code) => OnDispelStatusEffect?.Invoke(code);

        protected virtual void Update() { OnUpdate?.Invoke(); }
        
        
#if UNITY_EDITOR
        public virtual void SetUp()
        {
            if (characterName == string.Empty) Debug.LogError("CharacterName Required");
        }
#endif
    }
}
