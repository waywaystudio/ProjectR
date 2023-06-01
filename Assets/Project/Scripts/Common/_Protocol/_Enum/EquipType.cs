namespace Common
{
    public enum EquipType
    {
        None = 0,
        Weapon = DataIndex.Weapon,
        Head = DataIndex.Head,
        Top = DataIndex.Top,
        Glove = DataIndex.Glove,
        Bottom = DataIndex.Bottom,
        // Trinket
        // Earring,
        // Ring,
        // ViceStone,
        // ...
    }
    
    public enum EquipmentSlotType
    {
        None = 0,
        Weapon = DataIndex.Weapon,
        Head = DataIndex.Head,
        Top = DataIndex.Top,
        Glove = DataIndex.Glove,
        Bottom = DataIndex.Bottom,
    }

    public enum EnchantType
    {
        None = 0,
        PrimeEnchant = 1,
        SubEnchant = 2,
        ExtraEnchant = 3,
    }
}
