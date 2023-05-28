namespace Common
{
    public enum StatType
    {
        None,
        Power = DataIndex.Power, // 13010001,
        Health = DataIndex.Health, // 13010002,
        CriticalChance = DataIndex.CriticalChance, // 13010003,
        CriticalDamage = DataIndex.CriticalDamage, // 13010004,
        Haste = DataIndex.Haste, // 13010005,
        Mastery = DataIndex.Mastery, // 13020001,
        Retention = DataIndex.Retention, // 13020002,
        Armor = DataIndex.Armor, // 13020003,
        MoveSpeed = DataIndex.MoveSpeed, // 13020004,
        MaxResource = DataIndex.MaxResource, // 13030001,
        MinDamage = DataIndex.MinDamage, // 13030002,
        MaxDamage = DataIndex.MaxDamage, // 13030003,
    }

    public static class StatExtension
    {
        public static DataIndex ConvertToDataIndex(this StatType type) => (DataIndex)type;
        public static int GetHierarchyIndex(this StatType type) => type.ConvertToDataIndex().GetSubCategoryBar();

        public static bool IsPrimary(this StatType type) => GetHierarchyIndex(type)   == 01;
        public static bool IsSecondary(this StatType type) => GetHierarchyIndex(type) == 02;
        public static bool IsSubStat(this StatType type) => GetHierarchyIndex(type) == 03;
    }
}