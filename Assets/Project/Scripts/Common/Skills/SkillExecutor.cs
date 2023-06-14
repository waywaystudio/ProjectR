using Common.Execution;
using UnityEngine;

namespace Common.Skills
{
    public class SkillExecutor : MonoBehaviour, IEditable
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
            if (!Table.ContainKey(exe.Group)) return;
            
            table[exe.Group].Remove(exe);
        }

        public void Clear() => Table.Clear();


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            var executes = GetComponents<ExecuteComponent>();
            
            table.Clear();
            
            executes.ForEach(exe =>
            {
                if (!table.ContainKey(exe.Group))
                {
                    var executions = new Executions(exe);
                    
                    table.Add(exe.Group, executions);
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
