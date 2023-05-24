using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Common
{
    public class StatTable
    {
        #region Preset
        [ShowInInspector] public float Power => GetStatValue(StatType.Power);
        [ShowInInspector] public float Health => GetStatValue(StatType.Health);
        [ShowInInspector] public float CriticalChance => GetStatValue(StatType.CriticalChance);
        [ShowInInspector] public float CriticalDamage => GetStatValue(StatType.CriticalDamage);
        [ShowInInspector] public float Haste => GetStatValue(StatType.Haste);
        [ShowInInspector] public float Mastery => GetStatValue(StatType.Mastery);
        [ShowInInspector] public float Retention => GetStatValue(StatType.Retention);
        [ShowInInspector] public float Armor => GetStatValue(StatType.Armor);
        [ShowInInspector] public float MoveSpeed => GetStatValue(StatType.MoveSpeed);
        [ShowInInspector] public float MaxResource => GetStatValue(StatType.MaxResource);
        [ShowInInspector] public float MinWeaponValue => GetStatValue(StatType.MinDamage);
        [ShowInInspector] public float MaxWeaponValue => GetStatValue(StatType.MaxDamage);
        [ShowInInspector] public float MaxHp => Health * 10f;
        #endregion

        private Dictionary<StatType, StatSet> Table { get; } = new();
        private List<StatTable> ReferenceTable { get; } = new();

        public void RegisterTable(StatTable   anotherTable) => ReferenceTable.AddUniquely(anotherTable);
        public void UnregisterTable(StatTable anotherTable) => ReferenceTable.RemoveSafely(anotherTable);

        public void Add(StatSpec statSpec) => statSpec?.IterateOverStats(Add);
        public void Add(StatEntity stat)
        {
            if (stat == null) return;
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

        public void Remove(StatSpec statSpec) => statSpec?.IterateOverStats(Remove);
        public void Remove(StatEntity stat)
        {
            if (stat == null) return;
            if (!Table.ContainsKey(stat.StatType)) return;
            
            Table[stat.StatType].Remove(stat);
        }

        public void Clear() => Table.Clear();

        public float GetStatValue(StatType statType)
        {
            var result = !Table.ContainsKey(statType) ? 0f : Table[statType].Value;
            
            ReferenceTable.ForEach(otherTable =>
            {
                result += otherTable.GetStatValue(statType);
            });
        
            return result;
        }


        public class StatSet
        {
            private readonly Dictionary<string, StatEntity> table = new();
            public float Value { get; private set; }

            public void Add(StatEntity stat)
            {
                table[stat.StatKey] = stat;

                stat.OnValueChanged -= Calculate;
                stat.OnValueChanged += Calculate;
                Calculate(stat);
            }

            public void Remove(StatEntity stat)
            {
                var key = stat.StatKey;
                
                if (!table.ContainsKey(key)) return;
                
                stat.OnValueChanged -= Calculate;
                table.TryRemove(key);
                Calculate(stat);
            }
            

            private void Calculate(StatEntity stat)
            {
                Value = 0f;
                table.Values.ForEach(statValue => Value += statValue.Value);
            }
        }
    }
}
