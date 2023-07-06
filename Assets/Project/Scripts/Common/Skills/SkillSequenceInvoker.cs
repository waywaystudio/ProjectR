using UnityEngine;

namespace Common.Skills
{
    public class SkillSequenceInvoker : SequenceInvoker<Vector3>
    {
        public SkillSequenceInvoker(Sequencer<Vector3> sequencer) : base(sequencer) { }
        
        public void Execute() => Sequencer[Section.Execute].Invoke();
        public void Release() => Sequencer[Section.Release].Invoke();
    }
}
