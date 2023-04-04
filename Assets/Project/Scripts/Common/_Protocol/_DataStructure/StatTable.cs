using System.Collections.Generic;

namespace Common
{
    public class StatTable
    {
        private Dictionary<StatType, StatSet> statTable { get; } = new();

        public void Add(Spec spec) => spec.Iterate(stat => Add(spec.Key, stat));
        public void Add(string key, Stat stat)
        {
            if (statTable.ContainsKey(stat.StatType))
            {
                statTable[stat.StatType].Add(key, stat);
            }
            else
            {
                var newTable = new StatSet();
                newTable.Add(key, stat);
                
                statTable.Add(stat.StatType, newTable);
            }
        }
        
        public void Remove(Spec spec) => spec.Iterate(stat => Remove(spec.Key, stat));
        public void Remove(string key, Stat stat)
        {
            if (!statTable.ContainsKey(stat.StatType)) return;
            
            statTable[stat.StatType].Remove(key);
        }
        
        public float GetStatValue(StatType statType)
        {
            if (!statTable.ContainsKey(statType)) return 0;
        
            return statTable[statType].Value;
        }
    }
}
