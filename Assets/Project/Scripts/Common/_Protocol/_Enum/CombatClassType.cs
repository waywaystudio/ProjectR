using System;

namespace Common
{
    [Flags]
    public enum CombatClassType
    {
        None = 0,
        Knight = 1  << 0,
        Warrior = 1 << 1,
        Rogue = 1   << 2,
        Monk = 1    << 3,
        Hunter = 1  << 4,
        Mage = 1    << 5,
        Priest = 1  << 6,
        Bard = 1    << 7,
        Boss = 1    << 29,
        Minion = 1  << 30,
        
        /* Preset */
        Tank = Knight | Warrior,
        Melee = Rogue | Monk,
        Range = Hunter | Mage,
        Heal = Priest | Bard,
        Monster = Boss | Minion,
        
        All = int.MaxValue
    }

    public static class CombatClassTypeExtension
    {
        public static CombatClassType NextClass(this CombatClassType type)
        {
            var shiftOrder = GetBitPosition((int)type);

            if (shiftOrder == 30) shiftOrder = -1;

            return (CombatClassType)(1 << shiftOrder + 1);
        }
        
        public static int GetBitPosition(int value)
        {
            var position = 0;
            
            while (value != 0)
            {
                value >>= 1;
                position++;
            }
            
            return position - 1;
        }
    }
}

