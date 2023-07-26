using UnityEngine;

namespace Common.Execution.Hits
{
    public class HealHit : HitExecution, IEditable
    {
        [SerializeField] private StatSpec healSpec;

        public const string HealExecutorKey = "HealExecutorKey";
        public StatSpec HealSpec => healSpec;
        
        
        public override void Hit(ICombatTaker taker)
        {
            if (taker == null || !taker.Alive.Value) return;

            var entity        = new CombatLog(Sender.Provider, Sender.DataIndex, taker);
            var providerTable = Sender.Provider.StatTable;
            
            entity.Type = CombatEntityType.Heal;

            // Heal Creator
            var weaponAverage = Random.Range(providerTable.MinWeaponValue, providerTable.MaxWeaponValue);
            var healAmount = weaponAverage 
                  * (1.0f + providerTable.Power * 0.01f) 
                  * (1.0f + healSpec.Power      * 0.01f);

            // Critical Calculation : CombatFormula;
            if (CombatFormula.IsCritical(providerTable.CriticalChance + healSpec.CriticalChance))
            {
                entity.IsCritical =  true;
                healAmount        *= 1.0f + (100 + providerTable.CriticalDamage + healSpec.CriticalDamage) * 0.01f;
                entity.Type       =  CombatEntityType.CriticalHeal;
            }

            entity.Value =  healAmount;

            // OverHeal Calculation
            var remainHp = taker.StatTable.MaxHp - taker.Hp.Value;
            
            if (healAmount >= remainHp)
            {
                taker.Hp.Value =  taker.StatTable.MaxHp;
                entity.Value   -= remainHp;
            }
            else
            {
                taker.Hp.Value += healAmount;
            }

            taker.OnCombatTaken.Invoke(entity);
            Sender.Provider.OnCombatProvided.Invoke(entity);
        }
        
        
#if UNITY_EDITOR
        public void EditorSetUp()
        {
            healSpec.Clear();
            healSpec.Add(StatType.Power, HealExecutorKey, 100f);
            healSpec.Add(StatType.CriticalChance, HealExecutorKey, 0f);
            healSpec.Add(StatType.CriticalDamage, HealExecutorKey, 0f);
        }
#endif
    }
}
