using System;
using UnityEngine;

namespace Sequences
{
    public class Condition : MonoBehaviour
    {
        private ConditionTable ConditionTable { get; set; } = new();

        public bool IsAllTrue => ConditionTable.IsAllTrue;
        public bool HasFalse => ConditionTable.HasFalse;
        
        public void Add(string key, Func<bool> action) => ConditionTable.Add(key, action);
        public void Add(ConditionTable conditionTable)
        {
            // conditionTable.ForEach(condition => ConditionTable.Add(condition.Key, condition.Value));
        }

        public void Remove(string key) => ConditionTable.Remove(key);
        public void Clear() => ConditionTable.Clear();
    }
}
