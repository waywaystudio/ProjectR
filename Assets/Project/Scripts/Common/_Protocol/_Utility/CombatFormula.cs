using UnityEngine;

namespace Common
{
    public static class CombatFormula
    {
        /// <summary>
        /// Haste Additional Value, for reduce Casting Time, BuffTick
        /// </summary>
        /// <returns>usually less than 1.0f value</returns>
        public static float GetHasteValue(float haste) => 100f * (1f / (100 * (1f + haste * 0.01f)));
        
        /// <summary>
        /// Reduce Damage by Armor
        /// </summary>
        /// <returns>Reduced Damage</returns>
        public static float ArmorReduce(float armor)
        {
            return armor * 0.001f / (1f + 0.001f * armor);
        }

        /// <summary>
        /// Critical Chance Check. Must Critical is available
        /// </summary>
        /// <returns>is critical success</returns>
        public static bool IsCritical(float criticalChance)
        {
            // 100% 크리티컬 구현
            var mustCritical = criticalChance > 100f;
            return mustCritical || Random.Range(0f, 100f) < criticalChance;
        }
    }
}
