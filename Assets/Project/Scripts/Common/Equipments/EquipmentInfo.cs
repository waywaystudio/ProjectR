using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Common.Equipments
{
    [Serializable]
    public class EquipmentInfo
    {
        [SerializeField] private DataIndex dataCode;
        [SerializeField] private int enchantLevel;
        // Additional Save Data Below

        public DataIndex ActionCode => dataCode;
        public EquipType EquipType => (EquipType)ActionCode.GetCategory();
        public int EnchantLevel { get => enchantLevel; set => enchantLevel = value; }

        public EquipmentInfo(DataIndex dataCode, int enchantLevel = 0)
        {
            this.dataCode     = dataCode;
            this.enchantLevel = enchantLevel;
        }

        public static Equipment CreateEquipment(EquipmentInfo info, Transform parent)
        {
            if (info.ActionCode == DataIndex.None) return null;
            if (!Database.EquipmentMaster.GetObject(info.ActionCode, out var equipmentPrefab))
            {
                Debug.LogWarning($"Not Exist {info.ActionCode} prefab. return out Null");
                return null;
            }
            
            var equipObject = Object.Instantiate(equipmentPrefab, parent);
            
            if (!equipObject.TryGetComponent(out Equipment equipment))
            {
                Debug.LogWarning($"Not Exist Equipment script in {equipmentPrefab.name} GameObject");
                return null;
            }
            
            equipment.Enchant(info.EnchantLevel);
            return equipment;
        }


#if UNITY_EDITOR
        public void SetDataCode(DataIndex dataCode) => this.dataCode = dataCode;
#endif
    }
}
