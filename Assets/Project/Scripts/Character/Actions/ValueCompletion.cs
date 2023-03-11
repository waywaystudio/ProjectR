using UnityEngine;

namespace Character.Actions
{
    public class ValueCompletion : MonoBehaviour, ICombatTable
    {
        [SerializeField] private PowerValue power = new();
        
        public ICombatProvider Provider { get; private set; }
        public DataIndex ActionCode { get; private set; }
        public StatTable StatTable { get; } = new();
        
        public void UpdateStatTable()
        {
            StatTable.Clear();
            StatTable.Register(ActionCode, power);
            StatTable.UnionWith(Provider.StatTable);
        }

        public void Damage(ICombatTaker taker) => taker.TakeDamage(this);
        public void Heal(ICombatTaker taker) => taker.TakeHeal(this);
        

        public void Initialize(ICombatProvider provider, DataIndex actionCode)
        {
            Provider   = provider;
            ActionCode = actionCode;
        }
        
        public void SetPower(float value) => power.Value = value;
    }
}
