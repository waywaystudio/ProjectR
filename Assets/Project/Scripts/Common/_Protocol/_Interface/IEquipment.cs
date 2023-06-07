namespace Common
{
    public interface IItem : IDataIndexer
    {
        // + DataIndex DataIndex { get; set; }
        string ItemName { get; set; }
        UnityEngine.Sprite Icon { get; set; }

        void Create(DataIndex dataIndex);
    }
    
    public interface IEquipment : IItem
    {
        // + DataIndex DataIndex { get; set; }
        // + string ItemName { get; set; }
        // + UnityEngine.Sprite Icon { get; set; }
        
        EquipType EquipType { get; set; }
        int Tier { get; set; }
        int Level { get; set; }
        StatSpec StatSpec { get; set; }

        void Generate();
        void Upgrade();
        void Evolve();
    }
}
