using System.Collections.Generic;

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

        private Dictionary<StatType, StatSet> Table { get; } = new();

        public void Add(StatTable anotherTable)
        {
            anotherTable.Table.ForEach(tableElement =>
            {
                if (Table.TryGetValue(tableElement.Key, out var value))
                {
                    value.Add(tableElement.Value);
                }
                else
                {
                    Table.Add(tableElement.Key, tableElement.Value);
                }
            });
        }
        public void Add(Spec spec) => spec.IterateOverStats(Add);
        public void Add(Stat stat)
        {
            if (Table.TryGetValue(stat.StatType, out var value))
            {
                value.Add(stat);
            }
            else
            {
                var newTable = new StatSet();
                newTable.Add(stat);
                
                Table.Add(stat.StatType, newTable);
            }
        }

        public void Remove(StatTable anotherTable)
        {
            anotherTable.Table.ForEach(tableElement =>
            {
                if (Table.TryGetValue(tableElement.Key, out var value))
                {
                    value.Remove(tableElement.Value);
                }
            });
        }
        public void Remove(Spec spec) => spec.IterateOverStats(Remove);
        public void Remove(Stat stat)
        {
            if (!Table.ContainsKey(stat.StatType)) return;
            
            Table[stat.StatType].Remove(stat);
        }

        public void Remove(StatType type, string statKey)
        {
            if (!Table.ContainsKey(type)) return;
            
            Table[type].Remove(statKey);
        }

        public void Clear() => Table.Clear();

        public float GetStatValue(StatType statType)
        {
            if (!Table.ContainsKey(statType)) return 0;
        
            return Table[statType].Value;
        }
        
        public class StatSet
        {
            private readonly Dictionary<string, Stat> table = new();
            private float totalBaseValue;
            private float totalMultiValue;

            public float Value => totalBaseValue * (1 + totalMultiValue);


            public void Add(StatSet otherSet) => otherSet.table.ForEach(otherSetElement 
                => Add(otherSetElement.Value));
            public void Add(Stat stat)
            {
                if (table.ContainsKey(stat.StatKey))
                {
                    RemovePreviousStat(table[stat.StatKey]);
                    table[stat.StatKey] = stat;
                }
                else
                {
                    table.Add(stat.StatKey, stat);
                }
                
                AddNewStat(stat);
            }

            public void Remove(StatSet otherSet) => otherSet.table.ForEach(otherSetElement 
                => Remove(otherSetElement.Value));
            public void Remove(Stat stat)
            {
                var key = stat.StatKey;
                
                if (!table.ContainsKey(key)) return;
                
                RemovePreviousStat(table[key]);
                table.TryRemove(key);
            }
            public void Remove(string key)
            {
                if (!table.ContainsKey(key)) return;
                
                RemovePreviousStat(table[key]);
                table.TryRemove(key);
            }
            
            public void Clear() => table.Clear();


            private void RemovePreviousStat(Stat previousStat) => UpdateTotalValue(previousStat, true);
            private void AddNewStat(Stat newStat) => UpdateTotalValue(newStat);

            /// <param> False인 경우 amount를 type에 맞게 더해주며,
            /// True인 경우 원래대로 돌려 놓는다.
            ///     <name>isReverse</name>
            /// </param>
            private void UpdateTotalValue(Stat stat, bool isReverse = false)
            {
                var sign = isReverse ? -1.0f : 1.0f;
                totalBaseValue += stat.Value * sign;
            }
        }
    }
}
