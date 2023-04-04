using UnityEngine;

namespace Common.Execution
{
    public class DamageExecutor : ExecuteComponent, IEditable
    {
        [SerializeField] private DataIndex actionCode;
        [SerializeField] private PowerValue damage = new();
        // EntryPowerStat;
        // ExtraPowerStat;
        // EntryCriticalStat;
        // ExtraCriticalStat;

        private OldStatTable StatTable { get; } = new();


        public override void Execution(ICombatTaker taker, float instantMultiplier = 1f)
        {
            if (!taker.DynamicStatEntry.Alive.Value) return;
            
            UpdateStatTable();

            var entity       = new CombatEntity(taker);
            var damageAmount = StatTable.Power * instantMultiplier;
            
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
             
                Debug.Log($"{taker.Name} dead by {Executor.Provider.Name}'s {actionCode}");
                taker.Dead();
            }
            
            taker.DynamicStatEntry.Hp.Value -= damageAmount;

            Executor.Provider.OnDamageProvided.Invoke(entity);
            taker.OnDamageTaken.Invoke(entity);
        }
        
        
        private void UpdateStatTable()
        {
            StatTable.Clear();
            StatTable.Register(actionCode, damage);
            StatTable.UnionWith(Executor.Provider.StatTable);
        }

        private void OnEnable() { Executor?.ExecutionTable.Add(this); }
        private void OnDisable() { Executor?.ExecutionTable.Remove(this); }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            if (TryGetComponent(out Skills.SkillComponent skill))
            {
                actionCode   = skill.ActionCode;
                damage.Value = Database.SkillSheetData(actionCode).CompletionValueList[0];
                return;
            }
            
            if (TryGetComponent(out IDataIndexer indexer) && indexer.ActionCode is not DataIndex.None)
            {
                actionCode   = indexer.ActionCode;
                return;
            }

            Debug.LogError("At least SkillComponent or StatusEffectComponent In Same Inspector");
        }
#endif
    }
}
