using System;
using UnityEngine;

namespace Common.Equipments
{
    [Serializable]
    public class EquipmentEntity
    {
        public DataIndex DataIndex { get; set; } = DataIndex.None;
        public string ItemName { get; set; } = "None";
        public Sprite Icon { get; set; }
        public StatSpec ConstStatSpec { get; set; } = new();
        public EquipType EquipType { get; set; } = EquipType.None;
        public int Tier { get; set; }
        public int UpgradeLevel { get; set; } = 1;
        
        /* Ethos */
        public EthosEntity PrimeVice { get; set; } = null;
        public EthosEntity SubVice { get; set; } = null;
        public EthosEntity ExtraVice { get; set; } = null;

        public EquipmentEntity() { }
        public EquipmentEntity(DataIndex dataIndex)
        {
            Load(dataIndex);
        }

        public void Load(DataIndex dataIndex)
        {
            EquipmentUtility.LoadInstance(dataIndex, this);
        }

        public void Upgrade(int level)
        {
            EquipmentUtility.Upgrade(level, this);
        }
    }
}
