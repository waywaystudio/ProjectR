using System;
using UnityEngine;

namespace Sequences
{
    public class Condition : MonoBehaviour
    {
        private ConditionTable ConditionTable { get; set; } = new();

        public bool IsAllTrue => ConditionTable.IsAllTrue;
        public bool HasFalse => ConditionTable.HasFalse;
        
        public void Add(string key, Func<bool> action) => ConditionTable.Register(key, action);
        public void Add(ConditionTable conditionTable)
        {
            conditionTable.ForEach(condition => ConditionTable.TryAdd(condition.Key, condition.Value));
        }

        public void Remove(string key) => ConditionTable.Unregister(key);
        public void Clear() => ConditionTable.Clear();
    }
}
