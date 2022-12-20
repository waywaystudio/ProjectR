using UnityEngine;

namespace MainGame.Manager.Combat
{
    public class CombatManager : MonoBehaviour
    {
        /// <summary>
        /// Haste Additional Value, for reduce GlobalCoolTime, Casting Time, BuffDuration
        /// </summary>
        /// <param name="haste">Character.HasteTable.Result</param>
        /// <returns>usually less than 1.0f value</returns>
        public static float GetHasteValue(float haste) => 100f * (1f / (100 * (1f + haste)));

        /// <summary>
        /// Inversed Haste Additional Value for faster Animation Speed
        /// </summary>
        /// <param name="haste">Character.HasteTable.Result</param>
        /// <returns>usually more than 1.0f value</returns>
        public static float GetInverseHasteValue(float haste) => 1f + haste;

        /// <summary>
        /// Reduce Damage by Armor
        /// </summary>
        /// <returns>Reduced Damage</returns>
        public static float ArmorReduce(float armor, float damageValue)
        {
            return (float)(damageValue * (1f - armor * 0.001f / (1f + 0.001 * armor)));
        }

        private static bool IsHit(float hit, float evade)
        {
            // 100% 명중과 100% 회피 구현
            
            return Random.Range(0f, 1f) < hit - evade;
        }

        private static float IsCritical(float criticalChance)
        {
            // 100% 크리티컬 구현
            
            return Random.Range(0f, 1f) < criticalChance ? 2f : 1f;
        }

        
    }
}
