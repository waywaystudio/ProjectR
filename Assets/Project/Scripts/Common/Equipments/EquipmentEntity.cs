using System;
using UnityEngine;

namespace Common.Equipments
{
    [Serializable]
    public class EquipmentEntity
    {
        [SerializeField] private DataIndex dataIndex;
        [SerializeField] private CombatClassType availableClassType;
        [SerializeField] private string itemName;
        [SerializeField] private Sprite icon;
        [SerializeField] private Spec constSpec = new();

        public DataIndex DataIndex { get => dataIndex; set => dataIndex = value; }
        public CombatClassType AvailableClassType { get => availableClassType; set => availableClassType = value; }
        public EquipType EquipType => (EquipType)dataIndex.GetCategory();
        public string ItemName { get => itemName; set => itemName = value; }
        public string SerializeValue => $"{EquipType.ToString()}.{itemName}";
        public int UpgradeLevel { get; set; }
        public Sprite Icon { get => icon; set => icon = value; }
        public Spec DynamicSpec { get; set; } = new();
        public Spec ConstSpec { get => constSpec; set => constSpec = value; }
        public Spec Spec => constSpec + DynamicSpec;
        // Vice Spec


        public EquipmentEntity() { }
        public EquipmentEntity(DataIndex dataIndex)
        {
            Load(dataIndex);
        }
        

        public void Load(DataIndex dataIndex)
        {
            SetEntity(dataIndex);
            // Upgrade(UpgradeLevel);
        }

        public void SetEntity(DataIndex dataIndex)
        {
            EquipmentUtility.ImportEquipment(dataIndex, this);
        }

        // public void Upgrade()
        // {
        //     
        // }
    }
}
