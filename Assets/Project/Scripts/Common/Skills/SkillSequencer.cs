using UnityEngine;

namespace Common.Skills
{
    public class SkillSequencer : Sequencer<Vector3> { }
    public class SkillSequenceBuilder : SequenceBuilder<Vector3>
    {
        public SkillSequenceBuilder(Sequencer<Vector3> sequencer) : base(sequencer) { }
    }
}
