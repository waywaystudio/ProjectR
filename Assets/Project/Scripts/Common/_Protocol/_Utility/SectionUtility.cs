using System;
using Common.Skills;

namespace Common
{
    public static class SectionTypeExtension
    {
        public static Action GetInvokeAction(this Section type, SkillComponent skill) =>
            type switch
            {
                Section.Cancel   => skill.Invoker.Cancel,
                Section.Complete => skill.Invoker.Complete,
                Section.End      => skill.Invoker.End,
                Section.Execute  => skill.Invoker.Execute,
                Section.Release  => skill.Invoker.Release,
                _                    => null,
            };
    }
}
