using UnityEngine;

namespace Common.Completion
{
    public class DamageCompletion : MonoBehaviour
    {
        [SerializeField] private PowerValue damage = new();

        private ICombatProvider provider;
        private DataIndex actionCode;
        public StatTable StatTable { get; } = new();
        
        
        public void Initialize(ICombatProvider provider, DataIndex actionCode)
        {
            this.provider   = provider;
            this.actionCode = actionCode;
        }
        
        public void UpdateStatTable()
        {
            StatTable.Clear();
            StatTable.Register(actionCode, damage);
            StatTable.UnionWith(provider.StatTable);
        }

        public void Damage(ICombatTaker taker)
        {
            if (!taker.DynamicStatEntry.Alive.Value) return;
            
            UpdateStatTable();

            var entity       = new CombatEntity(taker);
            var damageAmount = StatTable.Power;
            
            // Critical Calculation : CombatFormula;
            if (CombatFormula.IsCritical(StatTable.Critical))
            {
                entity.IsCritical =  true;
                damageAmount      *= 2f;
            }
            
            // Armor Calculation : CombatFormula
            damageAmount = CombatFormula.ArmorReduce(taker.StatTable.Armor, damageAmount);
            entity.Value = damageAmount;

            // Dead Calculation
            if (damageAmount >= taker.DynamicStatEntry.Hp.Value)
            {
                taker.DynamicStatEntry.Hp.Value    =  0;
                taker.DynamicStatEntry.Alive.Value =  false;
                entity.Value                       -= taker.DynamicStatEntry.Hp.Value;
                entity.IsFinishedAttack            =  true;
             
                Debug.Log($"{taker.Name} dead by {provider.Name}'s {actionCode}");
                taker.Dead();
            }
            
            taker.DynamicStatEntry.Hp.Value -= damageAmount;

            provider.OnDamageProvided.Invoke(entity);
            taker.OnDamageTaken.Invoke(entity);
        }

        public void SetDamage(float value) => damage.Value = value;
    }
}
