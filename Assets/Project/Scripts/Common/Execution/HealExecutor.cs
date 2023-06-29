using UnityEngine;

namespace Common.Execution
{
    public class HealExecutor : ExecuteComponent, IEditable
    {
        [SerializeField] private DataIndex actionCode;
        [SerializeField] private StatSpec healSpec;
        
        public const string HealExecutorKey = "HealExecutorKey";
        public StatSpec HealSpec => healSpec;
        
        public override void Execution(ICombatTaker taker)
        {
            if (taker == null || !taker.DynamicStatEntry.Alive.Value) return;

            var entity        = new CombatEntity(actionCode, taker);
            var providerTable = Origin.Provider.StatTable;

            // Heal Creator
            var weaponAverage = Random.Range(providerTable.MinWeaponValue, providerTable.MaxWeaponValue);
            var healAmount = weaponAverage 
                  * (1.0f + providerTable.Power * 0.01f) 
                  * (1.0f + healSpec.Power      * 0.01f);

            // Critical Calculation : CombatFormula;
            if (CombatFormula.IsCritical(providerTable.CriticalChance + healSpec.CriticalChance))
            {
                entity.IsCritical =  true;
                healAmount      *= 1.0f + (100 + providerTable.CriticalDamage + healSpec.CriticalDamage) * 0.01f;
            }

            entity.Value =  healAmount;

            // Dead Calculation
            var remainHp = taker.StatTable.MaxHp - taker.DynamicStatEntry.Hp.Value;
            
            if (healAmount >= remainHp)
            {
                taker.DynamicStatEntry.Hp.Value =  taker.StatTable.MaxHp;
                entity.Value                    -= remainHp;
            }
            else
            {
                taker.DynamicStatEntry.Hp.Value += healAmount;
            }

            taker.OnHealTaken.Invoke(entity);
            Origin.Provider.OnHealProvided.Invoke(entity);
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
