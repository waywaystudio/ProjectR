using System;
using System.Collections.Generic;

namespace Common.Execution
{
    [Serializable]
    public class CombatExecutionList<T>
    {
        public List<T> List;

        public CombatExecutionList() => List = new List<T>();
        public CombatExecutionList(T component) => List = new List<T> { component };

        public void Add(T exe) => List.AddUniquely(exe);
        public void Remove(T exe) => List.RemoveSafely(exe);
        public void Clear() => List.Clear();
    }
}
