using System;

namespace Common
{
    [Flags]
    public enum CharacterActionMask
    {
        None = 0,
        Stop = 1      << 0,
        Run = 1       << 1,
        Dead = 1      << 2,
        KnockBack = 1 << 3,
        Stun = 1      << 4,
        Skill = 1     << 5,
        
        All = int.MaxValue
    }
}