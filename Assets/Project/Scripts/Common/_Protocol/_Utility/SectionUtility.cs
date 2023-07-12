using System;
using Common.Skills;

namespace Common
{
    public static class SectionExtension
    {
        public static Action GetCombatInvoker(this Section type, CombatSequenceInvoker invoker) =>
            type switch
            {
                Section.Cancel   => invoker.Cancel,
                Section.Complete => invoker.Complete,
                Section.End      => invoker.End,
                Section.Execute  => invoker.Execute,
                Section.Release  => invoker.Release,
                Section.Extra    => invoker.ExtraAction,
                _                => null,
            };
    }
}
