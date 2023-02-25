using Character.Data;
using Character.Data.BaseStats;
using Character.Skill;
using Core;
using UnityEngine;

namespace Character
{
    public class MonsterBehaviour : MonoBehaviour, ICombatExecutor
    {
        [SerializeField] protected string characterName = string.Empty;
        [SerializeField] protected DataIndex dataIndex;
        [SerializeField] protected RoleType role;
        [SerializeField] protected DynamicStatEntry dynamicStatEntry;
        [SerializeField] protected CharacterConstStats constStats;
        [SerializeField] protected ActionBehaviour actionBehaviour;

        [SerializeField] protected Transform damageSpawn;
        [SerializeField] protected Transform statusEffectHierarchy;

        public string Name => characterName ??= "Diablo";
        public RoleType Role => role;
        public DataIndex ActionCode => dataIndex;
        public IDynamicStatEntry DynamicStatEntry => dynamicStatEntry ??= GetComponentInChildren<DynamicStatEntry>();
        public ICombatProvider Provider => this;
        public GameObject Object => gameObject;
        public StatTable StatTable => DynamicStatEntry.StatTable;
        
        public ActionBehaviour ActionBehaviour => actionBehaviour;
        public Transform DamageSpawn => damageSpawn;
        public Transform StatusEffectHierarchy => statusEffectHierarchy;

        public ActionTable OnDead { get; } = new();
        public ActionTable<CombatEntity> OnTakeCombat { get; } = new();
        public ActionTable<CombatEntity> OnProvideCombat { get; } = new();
        public ActionTable<StatusEffectEntity> OnTakeStatusEffect { get; } = new();
        public ActionTable<StatusEffectEntity> OnProvideStatusEffect { get; } = new();
       

        public void Dead() => OnDead.Invoke();
        public void UpdateStatTable() { }
        public CombatEntity TakeDamage(ICombatTable combatTable) => CombatUtility.TakeDamage(combatTable, this);
        public CombatEntity TakeHeal(ICombatTable combatTable) => CombatUtility.TakeHeal(combatTable, this);
        public StatusEffectEntity TakeStatusEffect(IStatusEffect statusEffect) => CombatUtility.TakeStatusEffect(statusEffect, this);

        protected void Awake()
        {
            constStats       ??= GetComponentInChildren<CharacterConstStats>();
            dynamicStatEntry ??= GetComponentInChildren<DynamicStatEntry>();

            constStats.Initialize(StatTable);
            dynamicStatEntry.Initialize();
            OnDead.Register("DynamicAliveValue", () => dynamicStatEntry.IsAlive.Value = false);
        }
    }
}
