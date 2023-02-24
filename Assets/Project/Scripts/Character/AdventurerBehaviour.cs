using Character.Data;
using Character.Data.BaseStats;
using Character.Skill;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Character
{
    public class AdventurerBehaviour : MonoBehaviour, ICombatExecutor
    {
        public bool IsAutoStart;
        
        [SerializeField] protected string characterName = string.Empty;
        [SerializeField] protected RoleType role;
        [SerializeField] protected DataIndex combatClassID;
        [SerializeField] protected DynamicStatEntry dynamicStatEntry;
        [SerializeField] protected CharacterConstStats constStats;
        [SerializeField] protected ActionBehaviour actionBehaviour;
        [SerializeField] protected UnityEvent onManualControl;
        [SerializeField] protected UnityEvent onAutoControl;
        
        [SerializeField] protected Transform damageSpawn;
        [SerializeField] protected Transform statusEffectHierarchy;

        public ActionBehaviour ActionBehaviour => actionBehaviour;
        public Transform DamageSpawn => damageSpawn;
        public Transform StatusEffectHierarchy => statusEffectHierarchy;

        private bool IsAutoMode { get; set; }

        public string Name => characterName ??= "Diablo";
        public RoleType Role => role;
        public DataIndex ActionCode => combatClassID;
        public IDynamicStatEntry DynamicStatEntry => dynamicStatEntry ??= GetComponentInChildren<DynamicStatEntry>();
        public ICombatProvider Provider => this;
        public GameObject Object => gameObject;
        public StatTable StatTable => DynamicStatEntry.StatTable;
        public ActionTable OnDead { get; } = new();
        
        public ActionTable<CombatEntity> OnProvideCombat { get; } = new();
        public ActionTable<CombatEntity> OnTakeCombat { get; } = new();
        public ActionTable<StatusEffectEntity> OnProvideStatusEffect { get; } = new();
        public ActionTable<StatusEffectEntity> OnTakeStatusEffect { get; } = new();

        public void Dead() => OnDead.Invoke();
        public void UpdateStatTable() { }
        public CombatEntity TakeDamage(ICombatTable combatTable) => CombatUtility.TakeDamage(combatTable, this);
        public CombatEntity TakeHeal(ICombatTable combatTable) => CombatUtility.TakeHeal(combatTable, this);
        public StatusEffectEntity TakeStatusEffect(IStatusEffect statusEffect) => CombatUtility.TakeStatusEffect(statusEffect, this);


        public void OnFocused(AdventurerBehaviour focus)
        {
            if (focus == this)
            {
                IsAutoMode = false;
                onManualControl.Invoke();
            }
            else
            {
                onAutoControl.Invoke();
            }
        }
        
        protected void Awake()
        {
            constStats       ??= GetComponentInChildren<CharacterConstStats>();
            dynamicStatEntry ??= GetComponentInChildren<DynamicStatEntry>();
            actionBehaviour  ??= GetComponentInChildren<ActionBehaviour>();

            constStats.Initialize(StatTable);
            dynamicStatEntry.Initialize();
            OnDead.Register("DynamicAliveValue", () => dynamicStatEntry.IsAlive.Value = false);

            IsAutoMode = IsAutoStart;
            
            if (IsAutoMode) onAutoControl.Invoke();
            else
            {
                onManualControl.Invoke();
            }
        }
    }
}
