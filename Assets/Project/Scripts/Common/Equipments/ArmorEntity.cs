using UnityEngine;

namespace Common.Equipments
{
    public class ArmorEntity : IEquipment
    {
        public DataIndex DataIndex { get; set; }
        public EquipType EquipType { get; set; }
        public string ItemName { get; set; }
        public int Tier { get; set; }
        public int Level { get; set; } = 1;
        public Sprite Icon { get; set; }
        public StatSpec StatSpec { get; set; }
        
        private float Power => EquipmentUtility.GetArmorStat(DataIndex, StatType.Power, Level);
        private float Health => EquipmentUtility.GetArmorStat(DataIndex, StatType.Health, Level);
        private float Armor => EquipmentUtility.GetArmorStat(DataIndex, StatType.Armor, Level);
        private string SpecKey => EquipType.ToString();

        public ArmorEntity() { }
        public ArmorEntity(DataIndex index) => Create(index);

        public void Create(DataIndex dataIndex)
        {
            DataIndex = dataIndex;
            Icon      = Database.EquipmentSpriteData.Get(dataIndex);
            EquipType = (EquipType)dataIndex.GetCategory();
            Tier      = dataIndex.GetNumberOfDataIndex(3);
            ItemName  = EquipmentUtility.GetArmorName(DataIndex);

            CreateSpec();
        }
        
        public void Generate()
        {
            EquipType = (EquipType)DataIndex.GetCategory();
            Icon      = Database.EquipmentSpriteData.Get(DataIndex);
            Tier      = DataIndex.GetNumberOfDataIndex(3);
            ItemName  = EquipmentUtility.GetArmorName(DataIndex);

            UpdateSpec();
        }

        public void Upgrade()
        {
            if (Level >= 6) return;

            Level++;

            UpdateSpec();
        }

        public void Evolve()
        {
            if (Level != 6) return;
            if (Tier  >= 3) return;
            
            DataIndex += 101;
            Level     =  1;
            Tier      =  DataIndex.GetNumberOfDataIndex(3);
            Icon      =  Database.EquipmentSpriteData.Get(DataIndex);
            ItemName  =  EquipmentUtility.GetArmorName(DataIndex);

            UpdateSpec();
        }


        private void CreateSpec()
        {
            StatSpec = new StatSpec();
            
            StatSpec.Add(StatType.Power, SpecKey, Power);
            StatSpec.Add(StatType.Health, SpecKey, Health);
            StatSpec.Add(StatType.Armor, SpecKey, Armor);
        }
        
        private void UpdateSpec()
        {
            if (StatSpec == null) CreateSpec();
            
            StatSpec!.Change(StatType.Power, Power);
            StatSpec!.Change(StatType.Health, Health);
            StatSpec!.Change(StatType.Armor, Armor);
        }
    }
}
