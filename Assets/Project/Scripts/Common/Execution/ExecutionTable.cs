using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Execution
{
    public class ExecutionTable
    {
        [ShowInInspector]
        private Dictionary<ExecuteGroup, List<ExecuteComponent>> Table { get; }

        public ExecutionTable()
        {
            Table = new Dictionary<ExecuteGroup, List<ExecuteComponent>>();
        }

        public void Add(ExecuteComponent exr)
        {
            if (Table.TryGetValue(exr.Group, out var value))
            {
                value.Add(exr);
            }
            else
            {
                Table.Add(exr.Group, new List<ExecuteComponent> {exr});
            }
        }

        public void Remove(ExecuteComponent exr) => Remove(exr.Group);
        public void Remove(ExecuteGroup layer)
        {
            if (!Table.ContainsKey(layer)) return;

            Table.Remove(layer);
        }
        
        public void Clear() => Table.Clear();

        public void Execute(Vector3 position)
        {
            foreach (var exrList in Table)
            {
                foreach (var exr in exrList.Value)
                {
                    exr.Execution(position);
                }
            } 
        }

        public void Execute(ICombatTaker taker)
        {
            foreach (var exrList in Table)
            {
                foreach (var exr in exrList.Value)
                {
                    exr.Execution(taker);
                }
            }  
        }

        public void Execute(ExecuteGroup key, ICombatTaker taker)
        {
            foreach (var exrList in Table)
            {
                if (exrList.Key != key) continue;
                
                foreach (var exr in exrList.Value)
                {
                    exr.Execution(taker);
                }
            }
        }
    }
}
