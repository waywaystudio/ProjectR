using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    [Serializable] 
    public class StatSpec
    {
        #region Preset
        public float Power => GetStatValue(StatType.Power);
        public float Health => GetStatValue(StatType.Health);
        public float CriticalChance => GetStatValue(StatType.CriticalChance);
        public float CriticalDamage => GetStatValue(StatType.CriticalDamage);
        public float Haste => GetStatValue(StatType.Haste);
        public float Mastery => GetStatValue(StatType.Mastery);
        public float Retention => GetStatValue(StatType.Retention);
        public float Armor => GetStatValue(StatType.Armor);
        public float MoveSpeed => GetStatValue(StatType.MoveSpeed);
        public float MaxResource => GetStatValue(StatType.MaxResource);
        public float MinWeaponValue => GetStatValue(StatType.MinDamage);
        public float MaxWeaponValue => GetStatValue(StatType.MaxDamage);
        #endregion
        
        [SerializeField] private List<StatEntity> statList = new();

        public void Add(StatEntity stat) => statList.AddUniquely(stat);
        public void Add(StatType statType, string statKey, float value)
        {
            statList.Add(new StatEntity(statType, statKey, value));
            statList.Sort((x, y) => x.StatType.CompareTo(y.StatType));
        }

        public void Remove(StatEntity stat) => statList.RemoveSafely(stat);

        public void Clear() => statList.Clear();
        
        public void IterateOverStats(Action<StatEntity> action) => statList.ForEach(action.Invoke);
        
        public float GetStatValue(StatType statType)
        {
            var stat = statList.TryGetElement(element => element.StatType == statType);

            return stat is not null ? stat.Value : 0f;
        }

        public static StatSpec operator +(StatSpec a, StatSpec b)
        {
            var result = new StatSpec();
            
            a.IterateOverStats(result.Add);
            b.IterateOverStats(result.Add);

            return result;
        }
    }
}
