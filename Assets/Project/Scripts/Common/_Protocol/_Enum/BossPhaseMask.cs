using System;

namespace Common
{
    [Flags] 
    public enum BossPhaseMask
    {
        None = 0,
        
        Phase0 = 1 << 0,
        Phase1 = 1 << 1,
        Phase2 = 1 << 2,
        Phase3 = 1 << 3,
        Phase4 = 1 << 4,
        Phase5 = 1 << 5,
        Phase6 = 1 << 6,
        Phase7 = 1 << 7,
        Phase8 = 1 << 8,
        
        All = int.MaxValue
    }
}
