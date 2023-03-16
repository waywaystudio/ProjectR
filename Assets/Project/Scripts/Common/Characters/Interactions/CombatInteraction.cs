using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.Characters.Interactions
{
    public class CombatInteraction : MonoBehaviour
    {
        private ICombatTaker taker;

        public ICombatTaker Taker => taker ??= GetComponentInParent<ICombatTaker>();
        

        public void TakeDamage(ICombatTable combatTable)
        {
            if (!Taker.DynamicStatEntry.Alive.Value) return;
            
            combatTable.UpdateStatTable();

            var entity       = new CombatEntity(combatTable, Taker);
            var damageAmount = combatTable.StatTable.Power;
            
            // Critical Calculation
            if (IsCritical(combatTable.StatTable.Critical))
            {
                entity.IsCritical =  true;
                damageAmount            *= 2f;
            }
            
            // Armor Calculation
            damageAmount = ArmorReduce(Taker.StatTable.Armor, damageAmount);
            entity.Value = damageAmount;

            // Dead Calculation
            if (damageAmount >= Taker.DynamicStatEntry.Hp.Value)
            {
                Taker.DynamicStatEntry.Hp.Value    =  0;
                Taker.DynamicStatEntry.Alive.Value =  false;
                entity.Value                       -= Taker.DynamicStatEntry.Hp.Value;
                entity.IsFinishedAttack            =  true;
             
                Debug.Log($"{Taker.Name} dead by {combatTable.Provider.Name}'s {entity.CombatTable.ActionCode}");
                Taker.Dead();
            }
            
            Taker.DynamicStatEntry.Hp.Value -= damageAmount;

            combatTable.Provider.OnDamageProvided.Invoke(entity);
            Taker.OnDamageTaken.Invoke(entity);
        }

        public void TakeHeal(ICombatTable combatTable)
        {
            if (!Taker.DynamicStatEntry.Alive.Value) return;
            
            combatTable.UpdateStatTable();
            
            var entity     = new CombatEntity(combatTable, Taker);
            var healAmount = combatTable.StatTable.Power;

            // Critical Calculation
            if (IsCritical(combatTable.StatTable.Critical))
            {
                entity.IsCritical =  true;
                healAmount              *= 2f;
            }

            // OverHeal Calculation
            if (healAmount + Taker.DynamicStatEntry.Hp.Value >= Taker.StatTable.MaxHp)
            {
                healAmount = entity.Value = Taker.StatTable.MaxHp - Taker.DynamicStatEntry.Hp.Value;
            }

            Taker.DynamicStatEntry.Hp.Value += healAmount;
            
            combatTable.Provider.OnHealProvided.Invoke(entity);
            Taker.OnHealTaken.Invoke(entity);
        }

        public void TakeDeBuff(IStatusEffect statusEffect)
        {
            if (!Taker.DynamicStatEntry.Alive.Value) return;
            
            var entity = new StatusEffectEntity(statusEffect, Taker);
            
            var table  = Taker.DynamicStatEntry.DeBuffTable;
            table.Register(statusEffect);

            statusEffect.Provider.OnDeBuffProvided.Invoke(entity);
            Taker.OnDeBuffTaken.Invoke(entity);
        }

        public void TakeBuff(IStatusEffect statusEffect)
        {
            if (!Taker.DynamicStatEntry.Alive.Value) return;
            
            var entity = new StatusEffectEntity(statusEffect, Taker);
            var table  = Taker.DynamicStatEntry.BuffTable;
            
            table.Register(statusEffect);

            statusEffect.Provider.OnBuffProvided.Invoke(entity);
            Taker.OnBuffTaken.Invoke(entity);
        }
        
        
        /// <summary>
        /// Reduce Damage by Armor
        /// </summary>
        /// <returns>Reduced Damage</returns>
        private static float ArmorReduce(float armor, float damageValue)
        {
            return damageValue * (1f - armor * 0.001f / (1f + 0.001f * armor));
        }

        /// <summary>
        /// Critical Chance Check. Must Critical is available
        /// </summary>
        /// <returns>is critical success</returns>
        private static bool IsCritical(float criticalChance)
        {
            // 100% 크리티컬 구현
            var mustCritical = criticalChance > 1.0f;
            return mustCritical || Random.Range(0f, 1f) < criticalChance;
        }
    }
}
