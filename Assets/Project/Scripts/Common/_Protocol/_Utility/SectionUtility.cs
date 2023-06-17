using System;
using Common.Skills;

namespace Common
{
    public static class SectionTypeExtension
    {
        public static Action GetSkillAction(this SectionType type, SkillComponent skill) =>
            type switch
            {
                SectionType.Active => skill.SkillSequencer.Active,
                SectionType.Cancel     => skill.SkillSequencer.Cancel,
                SectionType.Complete   => skill.SkillSequencer.Complete,
                SectionType.End        => skill.SkillSequencer.End,
                SectionType.Execute    => skill.Execution,
                _                      => null,
            };
    }
}
