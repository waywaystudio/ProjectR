using System;
using Common.Skills;

namespace Common
{
    public static class SectionTypeExtension
    {
        public static Action GetInvokeAction(this SectionType type, SkillComponent skill) =>
            type switch
            {
                SectionType.Cancel   => skill.Invoker.Cancel,
                SectionType.Complete => skill.Invoker.Complete,
                SectionType.End      => skill.Invoker.End,
                SectionType.Execute  => skill.Invoker.Execute,
                SectionType.Release  => skill.Invoker.Release,
                _                    => null,
            };
    }
}
