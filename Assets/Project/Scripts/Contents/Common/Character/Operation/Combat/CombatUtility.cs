using UnityEngine;

namespace Common.Character.Operation.Combat
{
    public static class CombatUtility
    {
        /// <summary>
        /// Reduce Damage by Armor
        /// </summary>
        /// <returns>Reduced Damage</returns>
        public static float ArmorReduce(float armor, float damageValue)
        {
            return (float)(damageValue * (1f - armor * 0.001f / (1f + 0.001 * armor)));
        }

        /// <summary>
        /// Hit Chance Check. Must hit, Must evade are available
        /// </summary>
        /// <returns>is hit success</returns>
        public static bool IsHit(float hit, float evade)
        {
            // 100% 명중과 100% 회피 구현
            var mustHit = hit > 1.0f;
            var mustEvade = evade > 1.0f;

            if (mustHit ^ mustEvade == false)
            {
                return Random.Range(0f, 1f) <= hit - evade;
            }

            return mustHit;
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

        public static void TakeDamage(ICombatProvider provider, ICombatTaker taker)
        {
            // var log = new CombatLog
            // {
            //     Provider = provider.Name,
            //     Taker = CharacterName,
            //     SkillName = provider.ActionName,
            // };
            //
            // // Hit Chance
            // if (IsHit(provider.CombatValue.Hit, Evade))
            // {
            //     log.IsHit = true;
            // }
            // else
            // {
            //     log.IsHit = false;
            //     provider.CombatReport(log);
            //     return;
            // }
            //
            // var damageAmount = provider.CombatValue.Power;
            //
            // // Critical
            // if (IsCritical(provider.CombatValue.Critical))
            // {
            //     log.IsCritical = true;
            //     damageAmount *= 2f;
            // }
            //
            // // Armor
            // damageAmount = ArmorReduce(Armor, damageAmount);
            //
            // Hp -= damageAmount;
            // log.Value = damageAmount;
            //
            // if (Hp <= 0.0d)
            // {
            //     Debug.Log("Dead!");
            //     log.Value -= (float)Hp;
            //     taker.IsAlive = false;
            // }
            
            // provider.CombatReport(log);
        }
    }
}
