using System;
using UnityEngine;

namespace Common.Equipments
{
    [Serializable]
    public class EquipmentInfo
    {
        [SerializeField] private DataIndex dataCode;
        [SerializeField] private int enchantLevel;
        // Additional Save Data Below

        public DataIndex ActionCode => dataCode;
        public int EnchantLevel { get => enchantLevel; set => enchantLevel = value; }

        public EquipmentInfo(DataIndex dataCode, int enchantLevel = 0)
        {
            this.dataCode     = dataCode;
            this.enchantLevel = enchantLevel;
        }

#if UNITY_EDITOR
        public void SetDataCode(DataIndex dataCode) => this.dataCode = dataCode;
#endif
    }
}
