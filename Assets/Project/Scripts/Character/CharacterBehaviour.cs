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

        private ISkillBehaviour skillBehaviour;
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
        
        public OldActionTable OnUpdate { get; } = new();
        public OldActionTable OnIdle { get; } = new();
        public OldActionTable<Vector3, Action> OnWalk { get; } = new();
        public OldActionTable<Vector3, Action> OnRun { get; } = new();
        public OldActionTable<Vector3> OnTeleport { get; } = new();
        public OldActionTable OnDead { get; } = new();

        public OldActionTable<SkillObject> OnUseSkill { get; } = new(4);
        public OldActionTable OnActiveSkill { get; } = new(8);
        public OldActionTable OnCompleteSkill { get; } = new(4);
        public OldActionTable OnHitSkill { get; } = new(4);
        public OldActionTable OnCancelSkill { get; } = new(4);

        public OldActionTable<IStatusEffect> OnTakeStatusEffect { get; } = new(2);
        public OldActionTable<DataIndex> OnDispelStatusEffect { get; } = new(2);
        public OldActionTable<CombatLog> OnCombatActive { get; } = new(4);
        public OldActionTable<CombatLog> OnCombatPassive { get; } = new(4);

        public ISkillBehaviour SkillBehaviour => skillBehaviour ??= GetComponentInChildren<ISkillBehaviour>();
        public ISearching SearchingEngine => searchingEngine ??= GetComponentInChildren<ISearching>();
        public ITargeting TargetingEngine => targetingEngine ??= GetComponentInChildren<ITargeting>();
        public IPathfinding PathfindingEngine => pathfindingEngine ??= GetComponentInChildren<IPathfinding>();
        public ISkillInfo SkillInfo { get; set; }

        public void Idle() => OnIdle?.Invoke();
        public void Walk(Vector3 destination, Action pathCallback = null) => OnWalk?.Invoke(destination, pathCallback);
        public void Run(Vector3 destination, Action pathCallback = null) => OnRun?.Invoke(destination, pathCallback);
        public void Teleport(Vector3 destination) => OnTeleport?.Invoke(destination);
        public void Dead() => OnDead.Invoke();
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
