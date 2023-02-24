using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Character.Skill
{
    public class ValueCompletion : MonoBehaviour, ICombatTable
    {
        [SerializeField] private PowerValue power;
        
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
        public void Damage(List<ICombatTaker> takerList)
        {
            foreach(var taker in takerList) taker.TakeDamage(this);
        }

        public void Heal(ICombatTaker taker) => taker.TakeHeal(this);
        public void Heal(List<ICombatTaker> takerList)
        {
            foreach(var taker in takerList) taker.TakeHeal(this);
        }

        public void Initialize(ICombatProvider provider, DataIndex actionCode)
        {
            Provider   = provider;
            ActionCode = actionCode;
        }
        
        public void SetPower(float value) => power.Value = value;
    }
}
