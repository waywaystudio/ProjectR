using UnityEngine;

namespace Common.Skills
{
    public class SkillSequence : Sequencer<Vector3>
    {
        public ActionTable<ICombatTaker> TakingAction { get; } = new();
    }
    public class SkillSequenceBuilder : SequenceBuilder<Vector3>
    {
        public SkillSequenceBuilder(Sequencer<Vector3> sequencer) : base(sequencer) { }
    }
}
