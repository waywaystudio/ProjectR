using UnityEngine;

namespace Common.Skills
{
    public class SkillSequenceInvoker : SequenceInvoker<Vector3>
    {
        private Sequencer<Vector3> SkillSequencer => Sequencer;
    
        public void Execute() => SkillSequencer[SectionType.Execute].Invoke();
        public void Release() => SkillSequencer[SectionType.Release].Invoke();
    }
}
