using System;

namespace Common
{
    [Flags]
    public enum AvailableClass
    {
        None = 0,
        Knight = 1 << 0,
        Guardian = 1 << 1,
        Warrior = 1 << 2,
        Rogue = 1 << 3,
        Hunter = 1 << 4,
        Mage = 1 << 5,
        Priest = 1 << 6,
        Bard = 1 << 7,
        Boss = 1 << 8,
        
        All = int.MaxValue
    }
}
