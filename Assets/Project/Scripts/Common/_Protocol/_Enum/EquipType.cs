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
        Boot = DataIndex.Boot,
        Trinket = DataIndex.Trinket,
    }
    
    public enum EquipSlotIndex
    {
        None = 0,
        Weapon = DataIndex.Weapon,
        Head = DataIndex.Head,
        Top = DataIndex.Top,
        Glove = DataIndex.Glove,
        Bottom = DataIndex.Bottom,
        Boot = DataIndex.Boot,
        Trinket1 = DataIndex.Trinket * 10 + 1, // == 271,
        Trinket2 = DataIndex.Trinket * 10 + 2, // == 272,
    }
}
