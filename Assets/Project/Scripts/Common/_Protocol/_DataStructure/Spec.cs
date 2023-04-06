using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    [Serializable] 
    public class Spec
    {
        [SerializeField] private List<Stat> statList = new();
        
        public float Power => GetStatValue(StatType.Power);
        public float Health => GetStatValue(StatType.Health);
        public float CriticalChance => GetStatValue(StatType.CriticalChance);
        public float CriticalDamage => GetStatValue(StatType.CriticalDamage);
        public float Haste => GetStatValue(StatType.Haste);
        public float Armor => GetStatValue(StatType.Armor);
        public float MoveSpeed => GetStatValue(StatType.MoveSpeed);
        public float MaxHp => GetStatValue(StatType.MaxHp);
        public float MaxResource => GetStatValue(StatType.MaxResource);
        public float MinWeaponValue => GetStatValue(StatType.MinDamage);
        public float MaxWeaponValue => GetStatValue(StatType.MaxDamage);
        public float ExtraPower => GetStatValue(StatType.ExtraPower);
        public float ExtraCriticalChance => GetStatValue(StatType.ExtraCriticalChance);
        public float ExtraCriticalDamage => GetStatValue(StatType.ExtraCriticalDamage);


        public void Add(Stat stat) => statList.AddUniquely(stat);
        public void Add(StatType statType, StatApplyType applyType, float value)
        {
            statList.Add(new Stat(statType, applyType, value));
            statList.Sort((x, y) => x.StatType.CompareTo(y.StatType));
        }

        public void Remove(Stat stat) => statList.RemoveSafely(stat);
        public void Remove(StatType statType, float value)
        {
            foreach (var stat in statList)
            {
                if (stat.StatType != statType || Math.Abs(stat.Value - value) > 0.0001f) continue;
                
                statList.Remove(stat);
                statList.Sort((x, y) => x.StatType.CompareTo(y.StatType));
                return;
            }
        }

        public void Clear() => statList.Clear();
        public void Iterate(Action<Stat> action) => statList.ForEach(action.Invoke);
        public void Register(string key, StatTable table) => table.Add(key, this);
        public void Unregister(string key, StatTable table) => table.Remove(key, this);


        private float GetStatValue(StatType statType)
        {
            foreach (var stat in statList)
            {
                if (stat.StatType == statType) return stat.Value;
            }

            return 0f;
        }
    }
}
