using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Common
{
    public class StatTable
    {
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
        
        [ShowInInspector]
        private Dictionary<StatType, StatSet> statTable { get; } = new();

        public void Add(StatTable anotherTable)
        {
            anotherTable.statTable.ForEach(tableElement =>
            {
                if (statTable.TryGetValue(tableElement.Key, out var value))
                {
                    value.Add(tableElement.Value);
                }
                else
                {
                    statTable.Add(tableElement.Key, tableElement.Value);
                }
            });
        }
        public void Add(string key, Spec spec) => spec.Iterate(stat => Add(key, stat));
        public void Add(string key, Stat stat)
        {
            if (statTable.TryGetValue(stat.StatType, out var value))
            {
                value.Add(key, stat);
            }
            else
            {
                var newTable = new StatSet();
                newTable.Add(key, stat);
                
                statTable.Add(stat.StatType, newTable);
            }
        }

        public void Remove(StatTable anotherTable)
        {
            anotherTable.statTable.ForEach(tableElement =>
            {
                if (statTable.TryGetValue(tableElement.Key, out var value))
                {
                    value.Remove(tableElement.Value);
                }
            });
        }
        public void Remove(string key, Spec spec) => spec.Iterate(stat => Remove(key, stat));
        public void Remove(string key, Stat stat)
        {
            if (!statTable.ContainsKey(stat.StatType)) return;
            
            statTable[stat.StatType].Remove(key);
        }

        public void Clear() => statTable.Clear();
        
        
        private float GetStatValue(StatType statType)
        {
            if (!statTable.ContainsKey(statType)) return 0;
        
            return statTable[statType].Value;
        }
    }
}
