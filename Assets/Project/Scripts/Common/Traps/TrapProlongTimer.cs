using System;

namespace Common.Traps
{
    [Serializable]
    public class TrapProlongTimer : TimeTrigger
    {
        public void Initialize(Trap trap)
        {
            // Trap Ignore Weight
            SetCallback(trap.Invoker.Execute);
            
            trap.Builder
                 .Add(Section.Active, "TrapCasting", Play)
                 .Add(Section.Execute, "Complete", trap.Invoker.Complete)
                 .Add(Section.End, "StopProlongTimer", Stop);
        }
    }
}
