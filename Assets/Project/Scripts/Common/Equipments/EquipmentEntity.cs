using System;
using UnityEngine;

namespace Common.Equipments
{
    [Serializable]
    public class EquipmentEntity
    {
        public DataIndex DataIndex { get; set; } = DataIndex.None;
        public EquipType EquipType { get; set; } = EquipType.None;
        public string ItemName { get; set; } = "None";
        public int Tier { get; set; } = 1;
        public int UpgradeLevel { get; set; } = 1;
        public Sprite Icon { get; set; }
        public StatSpec ConstStatSpec { get; set; } = new();
        
        /* Ethos */
        public EthosEntity PrimeVice { get; set; }
        public EthosEntity SubVice { get; set; }
        public EthosEntity ExtraVice { get; set; }

        public EquipmentEntity() { }
        public EquipmentEntity(DataIndex dataIndex)
        {
            DataIndex = dataIndex;
        }

        public void Load(DataIndex dataIndex)
        {
            EquipmentUtility.LoadInstance(dataIndex, this);
        }

        public void Upgrade()
        {
            EquipmentUtility.Upgrade(this);
        }

        public void Enchant(EnchantType enchantType, EthosType viceType, int value) => GetEnchantedEthos(enchantType).SetEthos(viceType, value);
        public void DisEnchant(EnchantType enchantType) => Enchant(enchantType, EthosType.None, 0);

        public EthosEntity GetEnchantedEthos(EnchantType enchantType) =>
            enchantType switch
            {
                EnchantType.PrimeEnchant => PrimeVice,
                EnchantType.SubEnchant   => SubVice,
                EnchantType.ExtraEnchant => ExtraVice,
                _                        => null
            };
    }
}
