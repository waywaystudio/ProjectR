using Core;
using UnityEngine;

namespace Main.Manager.Combat
{
    public class CombatManager : MonoBehaviour
    {
        public void Damage(IDamageProvider damageInfo, ICombatTaker target)
        {
            // if (!IsHit(damageInfo.Hit, takerInfo.Evade)) return;
            // var critical = IsCritical(damageInfo.Critical);
            // takerInfo.Hp -= System.Math.Max(0d, damageInfo.Value * critical * takerInfo.AdditionalValue);
        }

        public void Heal(IHealProvider healInfo, IHealTaker takerInfo)
        {
            var critical = IsCritical(healInfo.Critical);

            takerInfo.Hp += healInfo.Value * critical * takerInfo.AdditionalValue;
        }

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
