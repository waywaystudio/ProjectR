using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    [Serializable] 
    public class Spec
    {
        [SerializeField] private List<Stat> statList = new();
        [SerializeField] private string key;

        public string Key => key;

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
        public void Register(StatTable table) => table.Add(this);
        public void Unregister(StatTable table) => table.Remove(this);
    }
}
