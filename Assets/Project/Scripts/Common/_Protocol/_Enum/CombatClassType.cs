using System;

namespace Common
{
    [Flags]
    public enum CombatClassType
    {
        None = 0,
        Knight = 1 << 0,
        Warrior = 1 << 1,
        Rogue = 1  << 2,
        Monk = 1 << 3,
        Hunter = 1 << 4,
        Mage = 1 << 5,
        Priest = 1 << 6,
        Bard = 1 << 7,
        Boss = 1   << 30,
        Minion = 1 << 31,
        
        /* Preset */
        Tank = Knight | Warrior,
        Melee = Rogue | Monk,
        Range = Hunter | Mage,
        Heal = Priest | Bard,
        Monster = Boss | Minion,
        
        All = int.MaxValue
    }
}