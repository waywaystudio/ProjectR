using System;
using UnityEngine;

namespace Common
{
    [Flags] public enum CombatClassType
    {
        None = 0,
        Knight = 1  << 0,
        Warrior = 1 << 1,
        Rogue = 1   << 2,
        Ranger = 1  << 3,
        Mage = 1    << 4,
        Priest = 1  << 5,
        Boss = 1    << 29,
        Minion = 1  << 30,
        
        /* Preset */
        All = int.MaxValue
    }

    [Flags] public enum RoleType
    {
        Tank = CombatClassType.Knight,
        Melee = CombatClassType.Warrior | CombatClassType.Rogue,
        Range = CombatClassType.Ranger   | CombatClassType.Mage,
        Heal = CombatClassType.Priest,
        Adventurer = CombatClassType.Knight | 
                     CombatClassType.Warrior | 
                     CombatClassType.Rogue | 
                     CombatClassType.Ranger | 
                     CombatClassType.Mage | 
                     CombatClassType.Priest,
        Monster = CombatClassType.Boss      | CombatClassType.Minion,
    }

    public static class CombatClassTypeExtension
    {
        public static CombatClassType NextAdventurer(this CombatClassType type)
        {
            if (!IsSingleAdventurer(type))
            {
                Debug.LogError($"{type} is not Adventurer Index, return None");
                return CombatClassType.None;
            }
            
            var shiftOrder = GetBitPosition((int)type);

            if (shiftOrder == 5) shiftOrder = -1;

            return (CombatClassType)(1 << shiftOrder + 1);
        }
        public static CombatClassType PrevAdventurer(this CombatClassType type)
        {
            if (!IsSingleAdventurer(type))
            {
                Debug.LogError($"{type} is not Adventurer Index, return None");
                return CombatClassType.None;
            }
            
            var shiftOrder = GetBitPosition((int)type);

            if (shiftOrder == 0) shiftOrder = 6;

            return (CombatClassType)(1 << shiftOrder - 1);
        }
        
        private static int GetBitPosition(int value)
        {
            if (!IsPowerOfTwo(value))
            {
                Debug.LogError($"{value} is not a power of 2");
                return -1;
            }
            
            var position = 0;
            
            while (value != 0)
            {
                value >>= 1;
                position++;
            }
            
            return position - 1;
        }
        private static bool IsPowerOfTwo(int value)
        {
            return value != 0 && (value & (value - 1)) == 0;
        }
        public static bool IsSingleAdventurer(this CombatClassType type)
        {
            var typeAsNumber = (int)type;

            return typeAsNumber is > 0 and <= 1 << 6 && IsPowerOfTwo(typeAsNumber);
        }
    }
}

