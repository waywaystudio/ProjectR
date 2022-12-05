using UnityEngine;

namespace MainGame.Manager.Combat
{
    public class CombatManager : MonoBehaviour
    {
        private static bool IsHit(float hit, float evade)
        {
            var hitChance = hit - evade;

            if (Random.Range(0f, 1f) < hitChance)
            {
                return true;
            }
            
            // Log : Evade
            return false;
        }

        private static float IsCritical(float criticalChance)
        {
            if (Random.Range(0f, 1f) < criticalChance)
            {
                // Log : Critical Hit
                return 2f;
            }

            // Log : Hit
            return 1f;
        }
    }
}
