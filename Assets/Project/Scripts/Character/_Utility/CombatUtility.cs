using Core;
using UnityEngine;

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

        public static void TakeDamage(ICombatTable entity, ICombatTaker taker)
        {
            if (!taker.DynamicStatEntry.IsAlive.Value) return;
            
            var log = new CombatLog(entity.Provider.Name, taker.Name, entity.Provider.ActionCode.ToString());

            // Hit Chance
            if (IsHit(entity.StatTable.Hit, taker.StatTable.Evade)) log.IsHit = true;
            else
            {
                log.IsHit = false;
                entity.Provider.OnCombatActive?.Invoke(log);
                taker.OnCombatPassive?.Invoke(log);
                return;
            }
            
            var damageAmount = entity.StatTable.Power;
            
            // Critical
            if (IsCritical(entity.StatTable.Critical))
            {
                log.IsCritical = true;
                damageAmount *= 2f;
            }
            
            // Armor
            damageAmount = ArmorReduce(taker.StatTable.Armor, damageAmount);
            
            log.Value = damageAmount;

            if (damageAmount >= taker.DynamicStatEntry.Hp.Value || taker.DynamicStatEntry.Hp.Value <= 0f)
            {
                Debug.Log("Dead!");
                taker.DynamicStatEntry.Hp.Value = 0;
                log.Value -= taker.DynamicStatEntry.Hp.Value;
                
                taker.Dead();
            }

            taker.DynamicStatEntry.Hp.Value -= damageAmount;
            
            entity.Provider.OnCombatActive?.Invoke(log);
            taker.OnCombatPassive?.Invoke(log);
        }
        
        public static void TakeSpell(ICombatTable entity, ICombatTaker taker)
        {
            if (!taker.DynamicStatEntry.IsAlive.Value) return;
            
            var log = new CombatLog(entity.Provider.Name, taker.Name, entity.Provider.ActionCode.ToString());
            
            // Hit Chance
            if (IsHit(entity.StatTable.Hit, taker.StatTable.Evade)) log.IsHit = true;
            else
            {
                log.IsHit = false;
                entity.Provider.OnCombatActive?.Invoke(log);
                taker.OnCombatPassive?.Invoke(log);
                return;
            }
            
            var damageAmount = entity.StatTable.Power;
            
            // Critical
            if (IsCritical(entity.StatTable.Critical))
            {
                log.IsCritical = true;
                damageAmount *= 2f;
            }
            
            // Armor :: TODO.현재는 주문과 물리공격에 대한 방어력을 계산에 같은 값을 사용하고 있다.
            // 주문방어에 대한 컨셉이 잡히면 수정하자.
            damageAmount = ArmorReduce(taker.StatTable.Armor, damageAmount);
            
            log.Value = damageAmount;

            if (damageAmount >= taker.DynamicStatEntry.Hp.Value || taker.DynamicStatEntry.Hp.Value <= 0f)
            {
                Debug.Log("Dead!");
                taker.DynamicStatEntry.Hp.Value = 0;
                log.Value -= taker.DynamicStatEntry.Hp.Value;
                taker.DynamicStatEntry.IsAlive.Value = false;
            }

            taker.DynamicStatEntry.Hp.Value -= damageAmount;
            
            entity.Provider.OnCombatActive?.Invoke(log);
            taker.OnCombatPassive?.Invoke(log);
        }
        public static void TakeHeal(ICombatTable entity, ICombatTaker taker)
        {
            if (!taker.DynamicStatEntry.IsAlive.Value) return;
            
            var log = new CombatLog(entity.Provider.Name, taker.Name, entity.Provider.ActionCode.ToString(), true);

            var healAmount = entity.StatTable.Power;
            
            // Critical
            if (IsCritical(entity.StatTable.Critical))
            {
                log.IsCritical = true;
                healAmount *= 2f;
            }

            if (healAmount + taker.DynamicStatEntry.Hp.Value >= taker.StatTable.MaxHp)
            {
                healAmount = log.Value = taker.StatTable.MaxHp - taker.DynamicStatEntry.Hp.Value;
            }

            taker.DynamicStatEntry.Hp.Value += healAmount;
            
            entity.Provider.OnCombatActive?.Invoke(log);
            taker.OnCombatPassive?.Invoke(log);
        }
    }
}
