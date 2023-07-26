using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.Execution.Hits
{
    public class DamageHit : HitExecution, IEditable
    {
        [SerializeField] private StatSpec damageSpec;
        
        public const string DamageExecutorKey = "DamageExecutorKey";
        public StatSpec DamageSpec => damageSpec;


        public override void Hit(ICombatTaker taker)
        {
            if (taker == null || !taker.Alive.Value) return;

            var log        = new CombatLog(Sender.Provider, Sender.DataIndex, taker);
            var providerTable = Sender.Provider.StatTable;

            log.Type = CombatEntityType.Damage;

            // Damage Creator
            var weaponAverage = Random.Range(providerTable.MinWeaponValue, providerTable.MaxWeaponValue);
            var damageAmount = weaponAverage 
                  * (1.0f + providerTable.Power * 0.01f) 
                  * (1.0f + DamageSpec.Power    * 0.01f);

            // Critical Calculation : CombatFormula;
            if (CombatFormula.IsCritical(providerTable.CriticalChance + DamageSpec.CriticalChance))
            {
                log.IsCritical =  true;
                damageAmount      *= 1.0f + (100 + providerTable.CriticalDamage + DamageSpec.CriticalDamage) * 0.01f;
                log.Type       =  CombatEntityType.CriticalDamage;
            }
            
            // Armor Calculation : CombatFormula
            var armorReduceRate = CombatFormula.ArmorReduce(taker.StatTable.Armor);
            
            // Global Generic Damage Adder
            // => 
            
            damageAmount *= 1.0f - armorReduceRate;
            log.Value =  damageAmount;
            
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
                    log.Type       =  CombatEntityType.Absorb;
                }
            }

            // Dead Calculation
            if (damageAmount >= taker.Hp.Value)
            {
                taker.Hp.Value    =  0;
                taker.Alive.Value =  false;
                log.Value      -= taker.Hp.Value;
                
             
                Debug.Log($"{taker.Name} dead by {Sender.Provider.Name}'s {Sender.DataIndex}'s {log.Value}");
                taker.DeadBehaviour.Dead();
            }

            taker.Hp.Value -= damageAmount;
            taker.OnCombatTaken.Invoke(log);

            Sender.Provider.OnCombatProvided.Invoke(log);
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
