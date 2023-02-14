using Character.Data;
using Character.Data.BaseStats;
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

        public string Name => characterName ??= "Diablo";
        public RoleType Role => role;
        public DataIndex ActionCode => dataIndex;
        public IDynamicStatEntry DynamicStatEntry => dynamicStatEntry;
        public ICombatProvider Provider => this;
        public GameObject Object => gameObject;
        public StatTable StatTable { get; } = new(1);

        public ActionTable OnDead { get; } = new();
        public ActionTable<IOldStatusEffect> OnTakeStatusEffect { get; } = new(2);
        public OldActionTable<DataIndex> OnDispelStatusEffect { get; } = new(2);
        public OldActionTable<CombatLog> OnCombatActive { get; } = new(4);
        public OldActionTable<CombatLog> OnCombatPassive { get; } = new(4);

        public void Dead() => OnDead.Invoke();
        public void TakeDamage(ICombatTable provider) => CombatUtility.TakeDamage(provider, this);
        public void TakeSpell(ICombatTable provider) => CombatUtility.TakeSpell(provider, this);
        public void TakeHeal(ICombatTable provider) => CombatUtility.TakeHeal(provider, this);
        public void TakeStatusEffect(IStatusEffect entity) => CombatUtility.TakeStatusEffect(entity, this);
        public void TakeDispel(IStatusEffect entity) { throw new System.NotImplementedException(); }

        public void OldTakeStatusEffect(IOldStatusEffect statusEffect) => OnTakeStatusEffect?.Invoke(statusEffect);
        public void OldDispelStatusEffect(DataIndex code) => OnDispelStatusEffect?.Invoke(code);
        
        protected void Awake()
        {
            constStats       ??= GetComponentInChildren<CharacterConstStats>();
            dynamicStatEntry ??= GetComponentInChildren<DynamicStatEntry>();

            constStats.Initialize(StatTable);
            dynamicStatEntry.Initialize(StatTable);
            
            OnDead.Register("DynamicAliveValue", () => dynamicStatEntry.IsAlive.Value = false);
        }
    }
}
