using System.Collections.Generic;
using Common.Execution;

namespace Common.Skills
{
    public class SkillExecutor
    {
        [Sirenix.OdinInspector.ShowInInspector]
        private Dictionary<ExecuteGroup, List<ExecuteComponent>> Table { get; }

        public SkillExecutor()
        {
            Table = new Dictionary<ExecuteGroup, List<ExecuteComponent>>();
        }

        public void Add(ExecuteComponent exr)
        {
            if (Table.ContainsKey(exr.Group))
            {
                Table[exr.Group].Add(exr);
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

        public void Execute(ICombatTaker taker) => Execute(taker, 1f);
        public void Execute(ICombatTaker taker, float instantMultiplier)
        {
            foreach (var exrList in Table)
            {
                foreach (var exr in exrList.Value)
                {
                    exr.Execution(taker, instantMultiplier);
                }
            }  
        }

        public void Execute(ExecuteGroup key, ICombatTaker taker) => Execute(key, taker, 1f);
        public void Execute(ExecuteGroup key, ICombatTaker taker, float instantMultiplier)
        {
            foreach (var exrList in Table)
            {
                if (exrList.Key != key) continue;
                
                foreach (var exr in exrList.Value)
                {
                    exr.Execution(taker, instantMultiplier);
                }
            }
        }
    }
}
