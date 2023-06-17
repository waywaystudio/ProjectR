using System;
using Common.Skills;

namespace Common
{
    public static class SectionTypeExtension
    {
        public static Action GetInvokeAction(this SectionType type, SkillComponent skill) =>
            type switch
            {
                SectionType.Cancel   => skill.SequenceInvoker.Cancel,
                SectionType.Complete => skill.SequenceInvoker.Complete,
                SectionType.End      => skill.SequenceInvoker.End,
                SectionType.Execute  => skill.Execution,
                SectionType.Release  => skill.SequenceInvoker.Release,
                _                    => null,
            };
    }
}
