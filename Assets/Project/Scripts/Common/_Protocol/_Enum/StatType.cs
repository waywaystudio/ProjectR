namespace Common
{
    public enum StatType
    {
        None,
        Power = DataIndex.Power,
        Health = DataIndex.Health,
        CriticalChance = DataIndex.CriticalChance,
        CriticalDamage = DataIndex.CriticalDamage,
        Haste = DataIndex.Haste,
        Mastery = DataIndex.Mastery,
        Retention = DataIndex.Retention,
        Armor = DataIndex.Armor,
        MoveSpeed = DataIndex.MoveSpeed,
        MaxResource = DataIndex.MaxResource,
        MinDamage = DataIndex.MinDamage,
        MaxDamage = DataIndex.MaxDamage,
    }

    public static class StatExtension
    {
        public static bool IsPrimary(this StatType type)
        {
            return type is StatType.Power or StatType.Health or StatType.Armor;
        }

        public static bool IsSecondary(this StatType type)
        {
            return type is StatType.CriticalChance 
                           or StatType.Haste 
                           or StatType.Mastery 
                           or StatType.Retention;
        }
    }
}