namespace Common
{
    [System.Flags] 
    public enum CharacterMask
    {
        None = 0,
        Knight = 1  << 0,
        Warrior = 1 << 1,
        Rogue = 1   << 2,
        Ranger = 1  << 3,
        Mage = 1    << 4,
        Priest = 1  << 5,
        Vummy = 1 << 28,
        Villain = 1    << 29,
        Vinion = 1  << 30,
        
        Tank = Knight,
        Melee = Warrior | Rogue,
        Range = Ranger  | Mage,
        Heal = Priest,
        Adventurer = Tank  | 
                     Melee | 
                     Range | 
                     Heal,
        Monster = Villain | Vinion,
        
        /* Preset */
        All = int.MaxValue
    }
}

