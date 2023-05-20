using UnityEngine;

namespace Common.Execution
{
    public class DamageExecutor : ExecuteComponent, IEditable
    {
        [SerializeField] private DataIndex actionCode;
        [SerializeField] private Spec damageSpec;

        private const string DamageExecutorKey = "DamageExecutorKey"; 

        public override void Execution(ICombatTaker taker, float instantMultiplier = 1f)
        {
            if (!taker.DynamicStatEntry.Alive.Value) return;

            var entity        = new CombatEntity(taker);
            var providerTable = Executor.Provider.StatTable;

            // Damage Creator
            var weaponAverage = Random.Range(providerTable.MinWeaponValue, providerTable.MaxWeaponValue);
            var damageAmount = weaponAverage 
                  * (1.0f + providerTable.Power   * 0.01f) 
                  * (1.0f + damageSpec.ExtraPower * 0.01f);

            // Critical Calculation : CombatFormula;
            if (CombatFormula.IsCritical(providerTable.CriticalChance + damageSpec.ExtraCriticalChance))
            {
                entity.IsCritical =  true;
                damageAmount      *= 1.0f + (100 + providerTable.CriticalDamage + damageSpec.CriticalDamage) * 0.01f;
            }
            
            // Armor Calculation : CombatFormula
            var armorReduceRate = CombatFormula.ArmorReduce(taker.StatTable.Armor);
            
            damageAmount *= 1.0f - armorReduceRate;
            entity.Value =  damageAmount;
            
            // Shield Calculation
            var takerShield = taker.DynamicStatEntry.Shield;
            
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
            if (damageAmount >= taker.DynamicStatEntry.Hp.Value)
            {
                taker.DynamicStatEntry.Hp.Value    =  0;
                taker.DynamicStatEntry.Alive.Value =  false;
                entity.Value                       -= taker.DynamicStatEntry.Hp.Value;
                entity.IsFinishedAttack            =  true;
             
                Debug.Log($"{taker.Name} dead by {Executor.Provider.Name}'s {actionCode}");
                taker.Dead();
            }
            
            taker.DynamicStatEntry.Hp.Value -= damageAmount;

            Executor.Provider.OnDamageProvided.Invoke(entity);
            taker.OnDamageTaken.Invoke(entity);
        }


        private void OnEnable() { Executor?.ExecutionTable.Add(this); }
        private void OnDisable() { Executor?.ExecutionTable.Remove(this); }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            damageSpec.Clear();
            damageSpec.Add(StatType.ExtraCriticalChance, DamageExecutorKey, 0f);
            damageSpec.Add(StatType.ExtraCriticalDamage, DamageExecutorKey, 0f);
            
            if (TryGetComponent(out Skills.SkillComponent skill))
            {
                actionCode   = skill.DataIndex;
                
                damageSpec.Add(StatType.ExtraPower, DamageExecutorKey, Database.SkillSheetData(actionCode).CompletionValueList[0]);
                return;
            }
            
            if (TryGetComponent(out IDataIndexer indexer) && indexer.DataIndex is not DataIndex.None)
            {
                actionCode   = indexer.DataIndex;
                return;
            }

            Debug.LogError("At least SkillComponent or StatusEffectComponent In Same Inspector");
        }
#endif
    }
}
