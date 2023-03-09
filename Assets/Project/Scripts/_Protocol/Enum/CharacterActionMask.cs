using System;

namespace Core
{
    [Flags]
    public enum CharacterActionMask
    {
        None = 1 << 0,
        Run = 1 << 1,
        Rotate = 1 << 2,
        Stop = 1 << 3,
        Dead = 1 << 4,
        KnockBack = 1 << 5,
        Stun = 1 << 7,
        Skill = 1 << 8,
        All = int.MaxValue
    }
}
