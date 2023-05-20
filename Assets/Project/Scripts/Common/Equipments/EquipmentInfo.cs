using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Common.Equipments
{
    [Serializable]
    public class EquipmentInfo
    {
        [SerializeField] private DataIndex dataCode;
        [SerializeField] private Spec constSpec;
        [SerializeField] private int enchantLevel;

        public DataIndex ActionCode => dataCode;
        public EquipType EquipType => (EquipType)ActionCode.GetCategory();
        public int EnchantLevel { get => enchantLevel; set => enchantLevel = value; }
        public Spec ConstSpec => constSpec;

        public EquipmentInfo(DataIndex dataCode, int enchantLevel = 0)
        {
            this.dataCode     = dataCode;
            this.enchantLevel = enchantLevel;
        }

        public static Equipment CreateEquipment(EquipmentInfo info, Transform parent)
        {
            if (info.ActionCode == DataIndex.None) return null;
            if (!Database.EquipmentPrefabData.GetObject(info.ActionCode, out var equipmentPrefab)) return null;
            
            var equipObject = Object.Instantiate(equipmentPrefab, parent);
            
            if (!equipObject.TryGetComponent(out Equipment equipment))
            {
                Debug.LogWarning($"Not Exist Equipment script in {equipmentPrefab.name} GameObject");
                return null;
            }

            return equipment;
        }


#if UNITY_EDITOR
        public void SetDataCode(DataIndex dataCode) => this.dataCode = dataCode;
#endif
    }
}
