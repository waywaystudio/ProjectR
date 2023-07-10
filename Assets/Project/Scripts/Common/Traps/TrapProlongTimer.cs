using System;

namespace Common.Traps
{
    [Serializable]
    public class TrapProlongTimer : CastTimer
    {
        public void Initialize(Trap trap)
        {
            trap.Builder
                 .Add(Section.Active, "TrapCasting", () => Play(castingTime, trap.Invoker.Execute))
                 .Add(Section.Execute, "Complete", trap.Invoker.Complete)
                 .Add(Section.End, "StopProlongTimer", Stop);
        }
    }
}
