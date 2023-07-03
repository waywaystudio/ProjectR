using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.Execution.Variants
{
    public class DamageExecution : TakerExecution, IEditable
    {
        [SerializeField] private DataIndex actionCode;
        [SerializeField] private StatSpec damageSpec;
        
        public const string DamageExecutorKey = "DamageExecutorKey";
        public StatSpec DamageSpec => damageSpec;


        public override void Execution(ICombatTaker taker)
        {
            if (taker == null || !taker.Alive.Value) return;

            var entity        = new CombatEntity(actionCode, taker);
            var providerTable = Origin.Provider.StatTable;

            // Damage Creator
            var weaponAverage = Random.Range(providerTable.MinWeaponValue, providerTable.MaxWeaponValue);
            var damageAmount = weaponAverage 
                  * (1.0f + providerTable.Power * 0.01f) 
                  * (1.0f + DamageSpec.Power    * 0.01f);

            // Critical Calculation : CombatFormula;
            if (CombatFormula.IsCritical(providerTable.CriticalChance + DamageSpec.CriticalChance))
            {
                entity.IsCritical =  true;
                damageAmount      *= 1.0f + (100 + providerTable.CriticalDamage + DamageSpec.CriticalDamage) * 0.01f;
            }
            
            // Armor Calculation : CombatFormula
            var armorReduceRate = CombatFormula.ArmorReduce(taker.StatTable.Armor);
            
            // Global Generic Damage Adder
            // => 
            
            damageAmount *= 1.0f - armorReduceRate;
            entity.Value =  damageAmount;
            
            // Shield Calculation
            var takerShield = taker.Shield;
            
            if (takerShield.Value > 0)
            {
                if (damageAmount > takerShield.Value)
                {
                    damageAmount      -= takerShield.Value;
                    takerShield.Value =  0f;
                }
                else
                {
                    takerShield.Value -= damageAmount;
                    damageAmount      =  0f;
                }
            }

            // Dead Calculation
            if (damageAmount >= taker.Hp.Value)
            {
                taker.Hp.Value    =  0;
                taker.Alive.Value =  false;
                entity.Value                       -= taker.Hp.Value;
                entity.IsFinishedAttack            =  true;
             
                Debug.Log($"{taker.Name} dead by {Origin.Provider.Name}'s {actionCode}'s {entity.Value}");
                taker.Dead();
            }
            
            taker.Hp.Value -= damageAmount;
            taker.OnDamageTaken.Invoke(entity);

            Origin.Provider.OnDamageProvided.Invoke(entity);
        }
        

#if UNITY_EDITOR
        public void EditorSetUp()
        {
            damageSpec.Clear();
            damageSpec.Add(StatType.Power, DamageExecutorKey, 0f);
        }
#endif
    }
}
