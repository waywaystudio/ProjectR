using System;
using Common.Skills;

namespace Common
{
    public static class SectionTypeExtension
    {
        public static Action GetInvokeAction(this SectionType type, SkillComponent skill) =>
            type switch
            {
                SectionType.Cancel   => skill.SkillInvoker.Cancel,
                SectionType.Complete => skill.SkillInvoker.Complete,
                SectionType.End      => skill.SkillInvoker.End,
                SectionType.Execute  => skill.SkillInvoker.Execute,
                SectionType.Release  => skill.SkillInvoker.Release,
                _                    => null,
            };
    }
}
