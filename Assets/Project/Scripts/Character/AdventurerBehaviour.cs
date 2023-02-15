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
        [SerializeField] protected string characterName = string.Empty;
        [SerializeField] protected RoleType role;
        [SerializeField] protected DataIndex combatClassID;
        [SerializeField] protected DynamicStatEntry dynamicStatEntry;
        [SerializeField] protected CharacterConstStats constStats;
        [SerializeField] protected ActionBehaviour actionBehaviour;
        [SerializeField] protected UnityEvent onManualControl;
        [SerializeField] protected UnityEvent onAutoControl;

        private bool IsAutoMode { get; set; }

        public string Name => characterName ??= "Diablo";
        public RoleType Role => role;
        public DataIndex ActionCode => combatClassID;
        public IDynamicStatEntry DynamicStatEntry => dynamicStatEntry;
        public ICombatProvider Provider => this;
        public GameObject Object => gameObject;
        public StatTable StatTable { get; } = new(1);
        public ActionTable OnDead { get; } = new();

        public void Dead() => OnDead.Invoke();
        public void ActiveSkill(DataIndex actionCode) => actionBehaviour.ActiveSkill(actionCode);
        public void TakeDamage(ICombatTable provider) => CombatUtility.TakeDamage(provider, this);
        public void TakeSpell(ICombatTable provider) => CombatUtility.TakeSpell(provider, this);
        public void TakeHeal(ICombatTable provider) => CombatUtility.TakeHeal(provider, this);
        public void TakeStatusEffect(IStatusEffect statusEffect) => CombatUtility.TakeStatusEffect(statusEffect, this);
        public void TakeDispel(IStatusEffect entity) => CombatUtility.TakeDispel(entity, this);

        public OldActionTable<CombatLog> OnCombatActive { get; } = new(4);
        public OldActionTable<CombatLog> OnCombatPassive { get; } = new(4);

        [Button]
        public void ToggleAutoControl()
        {
            if (IsAutoMode)
            {
                IsAutoMode = false;
                onManualControl.Invoke();
            }

            else
            {
                IsAutoMode = true;
                onAutoControl.Invoke();
            }
        }
        
        protected void Awake()
        {
            constStats       ??= GetComponentInChildren<CharacterConstStats>();
            dynamicStatEntry ??= GetComponentInChildren<DynamicStatEntry>();
            actionBehaviour  ??= GetComponentInChildren<ActionBehaviour>();

            constStats.Initialize(StatTable);
            dynamicStatEntry.Initialize(StatTable);
            OnDead.Register("DynamicAliveValue", () => dynamicStatEntry.IsAlive.Value = false);
        }
    }
}
