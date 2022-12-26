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
            var log = new CombatLog(provider.Name, taker.Name, provider.ActionName);

            // Hit Chance
            if (IsHit(provider.StatTable.Hit, taker.StatTable.Evade)) log.IsHit = true;
            else
            {
                log.IsHit = false;
                provider.ReportActive(log);
                taker.ReportPassive(log);
                return;
            }
            
            var damageAmount = provider.StatTable.Power;
            
            // Critical
            if (IsCritical(provider.StatTable.Critical))
            {
                log.IsCritical = true;
                damageAmount *= 2f;
            }
            
            // Armor
            damageAmount = ArmorReduce(taker.StatTable.Armor, damageAmount);
            
            log.Value = damageAmount;

            if (damageAmount >= taker.Hp || taker.Hp <= 0f)
            {
                Debug.Log("Dead!");
                taker.Hp = 0;
                log.Value -= taker.Hp;
                taker.IsAlive = false;
            }

            taker.Hp -= damageAmount;
            
            provider.ReportActive(log);
            taker.ReportPassive(log);
        }
        public static void TakeSpell(ICombatProvider provider, ICombatTaker taker)
        {
            var log = new CombatLog(provider.Name, taker.Name, provider.ActionName);
            
            // Hit Chance
            if (IsHit(provider.StatTable.Hit, taker.StatTable.Evade)) log.IsHit = true;
            else
            {
                log.IsHit = false;
                provider.ReportActive(log);
                taker.ReportPassive(log);
                return;
            }
            
            var damageAmount = provider.StatTable.Power;
            
            // Critical
            if (IsCritical(provider.StatTable.Critical))
            {
                log.IsCritical = true;
                damageAmount *= 2f;
            }
            
            // Armor :: TODO.현재는 주문과 물리공격에 대한 방어력을 계산에 같은 값을 사용하고 있다.
            // 주문방어에 대한 컨셉이 잡히면 수정하자.
            damageAmount = ArmorReduce(taker.StatTable.Armor, damageAmount);
            
            log.Value = damageAmount;

            if (damageAmount >= taker.Hp || taker.Hp <= 0f)
            {
                Debug.Log("Dead!");
                taker.Hp = 0;
                log.Value -= taker.Hp;
                taker.IsAlive = false;
            }

            taker.Hp -= damageAmount;
            
            provider.ReportActive(log);
            taker.ReportPassive(log);
        }
        public static void TakeHeal(ICombatProvider provider, ICombatTaker taker)
        {
            var log = new CombatLog(provider.Name, taker.Name, provider.ActionName, true);

            var healAmount = provider.StatTable.Power;
            
            // Critical
            if (IsCritical(provider.StatTable.Critical))
            {
                log.IsCritical = true;
                healAmount *= 2f;
            }

            if (healAmount + taker.Hp >= taker.StatTable.MaxHp)
            {
                healAmount = log.Value = taker.StatTable.MaxHp - taker.Hp;
            }

            taker.Hp += healAmount;
            
            provider.ReportActive(log);
            taker.ReportPassive(log);
        }
    }
}
