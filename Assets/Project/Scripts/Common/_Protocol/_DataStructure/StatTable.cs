using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Common
{
    public class StatTable
    {
        private readonly Dictionary<string, Stat> table = new();
        private float totalBaseValue;
        private float totalMultiValue;
        
        [ShowInInspector]
        public float Value => totalBaseValue * (1 + totalMultiValue);
        

        public void Add(string key, Stat stat)
        {
            if (table.ContainsKey(key))
            {
                RemovePreviousStat(table[key]);
                table[key] = stat;
            }
            else
            {
                table.Add(key, stat);
            }
            
            AddNewStat(stat);
        }

        public void Remove(string key)
        {
            if (!table.ContainsKey(key)) return;
            
            RemovePreviousStat(table[key]);
            table.TryRemove(key);
        }

        public void Clear()
        {
            table.Clear();
        }


        private void RemovePreviousStat(Stat previousStat) => UpdateTotalValue(previousStat, true);
        private void AddNewStat(Stat newStat) => UpdateTotalValue(newStat);

        /// <param> False인 경우 amount를 type에 맞게 더해주며,
        /// True인 경우 원래대로 돌려 놓는다.
        ///     <name>isReverse</name>
        /// </param>
        private void UpdateTotalValue(Stat stat, bool isReverse = false)
        {
            var sign = isReverse ? -1.0f : 1.0f;

            switch (stat.ApplyType)
            {
                case StatApplyType.Plus:            totalBaseValue  += stat.Value * sign; break;
                case StatApplyType.Minus:           totalBaseValue  -= stat.Value * sign; break;
                case StatApplyType.PercentIncrease: totalMultiValue += stat.Value * sign * 0.01f; break;
                case StatApplyType.PercentDecrease: totalMultiValue -= stat.Value * sign * 0.01f; break;
                case StatApplyType.None:            throw new ArgumentOutOfRangeException();
                default:                            throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    public class PowerStatTable : StatTable {}
    public class CriticalStatTable : StatTable {}
    public class HasteStatTable : StatTable {}
    public class MoveSpeedStatTable : StatTable {}
    public class ArmorStatTable : StatTable {}
    public class MaxHpStatTable : StatTable {}
    public class MaxResourceStatTable : StatTable {}
    public class MinWeaponStatTable : StatTable {}
    public class MaxWeaponStatTable : StatTable {}
}
