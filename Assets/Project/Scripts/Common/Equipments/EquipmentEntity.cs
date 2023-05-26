using System;
using UnityEngine;

namespace Common.Equipments
{
    [Serializable]
    public class EquipmentEntity
    {
        [SerializeField] private DataIndex dataIndex;
        [SerializeField] private string itemName;
        [SerializeField] private Sprite icon;
        [SerializeField] private StatSpec constStatSpec = new();
        
        public DataIndex DataIndex { get => dataIndex; set => dataIndex = value; }
        public EquipType EquipType => (EquipType)dataIndex.GetCategory();
        public string ItemName { get => itemName; set => itemName = value; }
        public Sprite Icon { get => icon; set => icon = value; }
        public StatSpec ConstStatSpec { get => constStatSpec; set => constStatSpec = value; }
        public int Tier => dataIndex.GetNumberOfDataIndex(4);
        
        /* Upgrade */
        public int UpgradeLevel { get; set; }
        public StatSpec UpgradeStatSpec { get; set; }


        /* Relic */ 
        [Sirenix.OdinInspector.ShowInInspector]
        public RelicTable RelicTable { get; set; } = new();
        public EthosSpec RelicEthosSpec => RelicTable.RelicEthos;
        public StatSpec RelicStatSpec => RelicTable.EnchantSpec;
        public RelicType CurrentRelicType => RelicTable.CurrentRelicType;

        public EquipmentEntity() { }
        public EquipmentEntity(DataIndex dataIndex)
        {
            Load(dataIndex);
        }
        

        public void Load(DataIndex dataIndex)
        {
            SetEntity(dataIndex);
            ChangeRelic(CurrentRelicType);
            Upgrade();
        }

        public void ChangeRelic(RelicType type)
        {
            // if (!RelicTable.IsUnlocked(type)) return;
            RelicTable.ChangeTo(type);
        }

        public void Upgrade()
        {
            
        }

        public void Enchant(RelicType    relicType) => RelicTable.Enchant(relicType, Tier, EquipType.ToString());
        public void Conversion(RelicType relicType) => RelicTable.Conversion(relicType, Tier, EquipType.ToString());

        public RelicEntity GetRelic(RelicType relicType) => RelicTable.GetRelic(relicType);
        
        
        private void SetEntity(DataIndex dataIndex)
        {
            EquipmentUtility.ImportEquipment(dataIndex, this);
        }
    }
}
