using UnityEngine;
using Random = UnityEngine.Random;

namespace Character
{
    public static class CombatUtility
    {
        /// <summary>
        /// Reduce Damage by Armor
        /// </summary>
        /// <returns>Reduced Damage</returns>
        public static float ArmorReduce(float armor, float damageValue)
        {
            return damageValue * (1f - armor * 0.001f / (1f + 0.001f * armor));
        }

        /// <summary>
        /// Critical Chance Check. Must Critical is available
        /// </summary>
        /// <returns>is critical success</returns>
        public static bool IsCritical(float criticalChance)
        {
            // 100% 크리티컬 구현
            var mustCritical = criticalChance > 1.0f;
            return mustCritical || Random.Range(0f, 1f) < criticalChance;
        }

        public static CombatEntity TakeDamage(ICombatTable combatTable, ICombatTaker taker)
        {
            if (!taker.DynamicStatEntry.Alive.Value) return null;
            
            combatTable.UpdateStatTable();
            
            var combatEntity = new CombatEntity(combatTable, taker);
            var damageAmount = combatTable.StatTable.Power;
            
            // Critical Calculation
            if (IsCritical(combatTable.StatTable.Critical))
            {
                combatEntity.IsCritical = true;
                damageAmount *= 2f;
            }
            
            // Armor Calculation
            damageAmount       = ArmorReduce(taker.StatTable.Armor, damageAmount);
            combatEntity.Value = damageAmount;

            if (damageAmount >= taker.DynamicStatEntry.Hp.Value)
            {
                taker.DynamicStatEntry.Hp.Value      =  0;
                taker.DynamicStatEntry.Alive.Value =  false;
                combatEntity.Value                   -= taker.DynamicStatEntry.Hp.Value;
             
                Debug.Log($"{taker.Name} dead by {combatEntity.CombatTable.Provider.Name}'s {combatEntity.CombatTable.ActionCode}");
                taker.CharacterBehaviour.Dead();
            }

            taker.DynamicStatEntry.Hp.Value -= damageAmount;
            
            combatTable.Provider.OnProvideDamage.Invoke(combatEntity);
            taker.OnTakeDamage.Invoke(combatEntity);

            return combatEntity;
        }
        
        public static CombatEntity TakeHeal(ICombatTable combatTable, ICombatTaker taker)
        {
            if (!taker.DynamicStatEntry.Alive.Value) return null;
            
            combatTable.UpdateStatTable();
            
            var combatEntity = new CombatEntity(combatTable, taker);
            var healAmount   = combatTable.StatTable.Power;

            // Critical Calculation
            if (IsCritical(combatTable.StatTable.Critical))
            {
                combatEntity.IsCritical = true;
                healAmount *= 2f;
            }

            // OverHeal Calculation
            if (healAmount + taker.DynamicStatEntry.Hp.Value >= taker.StatTable.MaxHp)
            {
                healAmount = combatEntity.Value = taker.StatTable.MaxHp - taker.DynamicStatEntry.Hp.Value;
            }

            taker.DynamicStatEntry.Hp.Value += healAmount;
            
            combatTable.Provider.OnProvideHeal.Invoke(combatEntity);
            taker.OnTakeHeal.Invoke(combatEntity);
            
            return combatEntity;
        }

        public static StatusEffectEntity TakeDeBuff(IStatusEffect statusEffect, ICombatTaker taker)
        {
            if (!taker.DynamicStatEntry.Alive.Value) return null;
            
            var statusEffectEntity = new StatusEffectEntity(statusEffect, taker);
            var table = taker.DynamicStatEntry.DeBuffTable;
            
            table.Register(statusEffect);

            statusEffect.Provider.OnProvideDeBuff.Invoke(statusEffectEntity);
            taker.OnTakeDeBuff.Invoke(statusEffectEntity);

            return statusEffectEntity;
        }
        
        public static StatusEffectEntity TakeBuff(IStatusEffect statusEffect, ICombatTaker taker)
        {
            if (!taker.DynamicStatEntry.Alive.Value) return null;
            
            var statusEffectEntity = new StatusEffectEntity(statusEffect, taker);
            var table = taker.DynamicStatEntry.BuffTable;
            
            table.Register(statusEffect);

            statusEffect.Provider.OnProvideBuff.Invoke(statusEffectEntity);
            taker.OnTakeBuff?.Invoke(statusEffectEntity);

            return statusEffectEntity;
        }
    }
}

/// <summary>
/// Hit Chance Check. Must hit, Must evade are available
/// </summary>
/// <returns>is hit success</returns>
// public static bool IsHit(float hit, float evade)
// {
//     // 100% 명중과 100% 회피 구현
//     var mustHit = hit > 1.0f;
//     var mustEvade = evade > 1.0f;
//
//     if (mustHit ^ mustEvade == false)
//     {
//         return Random.Range(0f, 1f) <= hit - evade;
//     }
//
//     return mustHit;
// }
