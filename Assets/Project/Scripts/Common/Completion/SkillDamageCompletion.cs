using Common.Skills;
using UnityEngine;

namespace Common.Completion
{
    public class SkillDamageCompletion : MonoBehaviour, IEditable
    {
        [SerializeField] private DataIndex actionCode;
        [SerializeField] private PowerValue damage = new();
        
        private SkillComponent skillComponent;
        
        private StatTable StatTable { get; } = new();
        private ICombatProvider Provider => skillComponent.Cb;


        public void Completion(ICombatTaker taker) => Completion(taker, 1.0f);
        public void Completion(ICombatTaker taker, float instantMultiplier)
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
             
                Debug.Log($"{taker.Name} dead by {Provider.Name}'s {actionCode}");
                taker.Dead();
            }
            
            taker.DynamicStatEntry.Hp.Value -= damageAmount;

            Provider.OnDamageProvided.Invoke(entity);
            taker.OnDamageTaken.Invoke(entity);
        }
        
        
        private void UpdateStatTable()
        {
            StatTable.Clear();
            StatTable.Register(actionCode, damage);
            StatTable.UnionWith(Provider.StatTable);
        }

        private void Awake()
        {
            if (!TryGetComponent(out skillComponent))
            {
                Debug.LogError("Require SkillComponent In Same Inspector");
            }
            
            skillComponent.OnCompletion.Register("Damage", Completion);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            if (!TryGetComponent(out skillComponent))
            {
                Debug.LogError("Require SkillComponent In Same Inspector");
            }

            actionCode   = skillComponent.ActionCode;
            damage.Value = Database.SkillSheetData(actionCode).CompletionValueList[0];
        }
#endif
    }
}
