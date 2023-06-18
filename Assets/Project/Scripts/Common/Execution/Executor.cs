using System;
using UnityEngine;

namespace Common.Execution
{
    [Serializable]
    public class Executor
    {
        [SerializeField] private Table<ExecuteGroup, Executions> table;
        
        public Table<ExecuteGroup, Executions> Table => table;

        public void Execute(ICombatTaker taker) => Execute(ExecuteGroup.Group1, taker);
        public void Execute(ExecuteGroup group, ICombatTaker taker)
        {
            table[group]?.ExecutionList.ForEach(exe => exe.Execution(taker));
        }
        
        public void Execute(Vector3 position) => Execute(ExecuteGroup.Group1, position);
        public void Execute(ExecuteGroup group, Vector3 position)
        {
            table[group]?.ExecutionList.ForEach(exe => exe.Execution(position));
        }
        
        public void Add(ExecuteComponent exe)
        {
            if (Table.TryGetValue(exe.Group, out var value))
            {
                value.Add(exe);
            }
            else
            {
                Table.Add(exe.Group, new Executions(exe));
            }
        }

        public void Remove(ExecuteComponent exe)
        {
            if (!Table.ContainsKey(exe.Group)) return;
            
            table[exe.Group].Remove(exe);
        }

        public void Clear() => Table.Clear();


#if UNITY_EDITOR
        public void EditorGetExecutions(GameObject parent)
        {
            var executes = parent.GetComponentsInChildren<ExecuteComponent>();
            
            table.Clear();
            
            executes.ForEach(exe =>
            {
                if (!table.ContainsKey(exe.Group))
                {
                    table.Add(exe.Group, new Executions(exe));
                }
                else
                {
                    table[exe.Group].ExecutionList.AddUniquely(exe);
                }
            });
        }
#endif
    }
}
