using System;
using UnityEngine;

namespace Common
{
    [Flags] public enum CharacterMask
    {
        None = 0,
        Knight = 1  << 0,
        Warrior = 1 << 1,
        Rogue = 1   << 2,
        Ranger = 1  << 3,
        Mage = 1    << 4,
        Priest = 1  << 5,
        Villain = 1    << 29,
        Vinion = 1  << 30,
        
        /* Preset */
        All = int.MaxValue
    }

    [Flags] public enum RoleType
    {
        Tank = CharacterMask.Knight,
        Melee = CharacterMask.Warrior | CharacterMask.Rogue,
        Range = CharacterMask.Ranger   | CharacterMask.Mage,
        Heal = CharacterMask.Priest,
        Adventurer = CharacterMask.Knight | 
                     CharacterMask.Warrior | 
                     CharacterMask.Rogue | 
                     CharacterMask.Ranger | 
                     CharacterMask.Mage | 
                     CharacterMask.Priest,
        Monster = CharacterMask.Villain      | CharacterMask.Vinion,
    }

    public static class CharacterMaskExtension
    {
        public static CharacterMask NextAdventurer(this CharacterMask type)
        {
            if (!IsSingleAdventurer(type))
            {
                Debug.LogError($"{type} is not Adventurer Index, return None");
                return CharacterMask.None;
            }
            
            var shiftOrder = GetBitPosition((int)type);

            if (shiftOrder == 5) shiftOrder = -1;

            return (CharacterMask)(1 << shiftOrder + 1);
        }
        public static CharacterMask PrevAdventurer(this CharacterMask type)
        {
            if (!IsSingleAdventurer(type))
            {
                Debug.LogError($"{type} is not Adventurer Index, return None");
                return CharacterMask.None;
            }
            
            var shiftOrder = GetBitPosition((int)type);

            if (shiftOrder == 0) shiftOrder = 6;

            return (CharacterMask)(1 << shiftOrder - 1);
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
        public static bool IsSingleAdventurer(this CharacterMask type)
        {
            var typeAsNumber = (int)type;

            return typeAsNumber is > 0 and <= 1 << 6 && IsPowerOfTwo(typeAsNumber);
        }
    }
}

