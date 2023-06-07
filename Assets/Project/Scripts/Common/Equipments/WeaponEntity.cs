using UnityEngine;

namespace Common.Equipments
{
    public class WeaponEntity : IEquipment
    {
        public DataIndex DataIndex { get; set; }
        public EquipType EquipType { get; set; }
        public string ItemName { get; set; }
        public int Tier { get; set; }
        public int Level { get; set; } = 1;
        public Sprite Icon { get; set; }
        public StatSpec StatSpec { get; set; }
        
        private float MinDamage => EquipmentUtility.GetWeaponStat(DataIndex, StatType.MinDamage, Level);
        private float MaxDamage => EquipmentUtility.GetWeaponStat(DataIndex, StatType.MaxDamage, Level);
        private float Power => EquipmentUtility.GetWeaponStat(DataIndex, StatType.Power, Level);
        private string SpecKey => EquipType.ToString();

        public WeaponEntity() { }
        public WeaponEntity(DataIndex index) => Create(index);

        public void Create(DataIndex dataIndex)
        {
            DataIndex = dataIndex;
            ItemName  = EquipmentUtility.GetWeaponName(DataIndex);
            Icon      = Database.EquipmentSpriteData.Get(DataIndex);
            EquipType = (EquipType)DataIndex.GetCategory();
            Tier      = DataIndex.GetNumberOfDataIndex(3);

            CreateSpec();
        }
        
        public void Generate()
        {
            // LoadData로 부터 DataIndex와 StatSpec과 Level을 받았다 치고.
            ItemName  = EquipmentUtility.GetWeaponName(DataIndex);
            Icon      = Database.EquipmentSpriteData.Get(DataIndex);
            EquipType = (EquipType)DataIndex.GetCategory();
            Tier      = DataIndex.GetNumberOfDataIndex(3);

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
            ItemName  =  EquipmentUtility.GetWeaponName(DataIndex);
            Icon      =  Database.EquipmentSpriteData.Get(DataIndex);
            Tier      =  DataIndex.GetNumberOfDataIndex(3);

            UpdateSpec();
        }


        private void CreateSpec()
        {
            StatSpec = new StatSpec();
            
            StatSpec.Add(StatType.MinDamage, SpecKey, MinDamage);
            StatSpec.Add(StatType.MaxDamage, SpecKey, MaxDamage);
            StatSpec.Add(StatType.Power, SpecKey, Power);
        }
        
        private void UpdateSpec()
        {
            if (StatSpec == null) CreateSpec();
            
            StatSpec!.Change(StatType.MinDamage, MinDamage);
            StatSpec!.Change(StatType.MaxDamage, MaxDamage);
            StatSpec!.Change(StatType.Power, Power);
        }
    }
}
