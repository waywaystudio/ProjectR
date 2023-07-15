using System;

namespace Common
{
    public interface ICombatObject : IActionSender
    {
        ICombatTaker Taker { get; }
        Func<float> Haste { get; }
        CombatSequence Sequence { get; }
        CombatSequenceInvoker Invoker { get; }
    }
}
