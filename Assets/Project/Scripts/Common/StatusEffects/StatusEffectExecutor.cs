using Common.Execution;
using UnityEngine;

namespace Common.StatusEffects
{
    public class StatusEffectExecutor : TakerExecutor
    {
        public void Initialize(Sequencer sequencer)
        {
            var builder = new SequenceBuilder(sequencer);
            
            if (takerExecutionTable.ContainsKey(ExecuteGroup.Group1))
            {
                // builder.Add(SectionType.Execute1, "MainExecutionGroup1", () => ToTaker())
            }
        }
    }
}
