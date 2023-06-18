using UnityEngine;

namespace Common.Skills
{
    public class SkillSequenceInvoker : SequenceInvoker<Vector3>
    {
        public SkillSequenceInvoker(Sequencer<Vector3> sequencer) => Sequencer = sequencer;
        
        public void Execute() => Sequencer[SectionType.Execute].Invoke();
        public void Release() => Sequencer[SectionType.Release].Invoke();
    }
}
