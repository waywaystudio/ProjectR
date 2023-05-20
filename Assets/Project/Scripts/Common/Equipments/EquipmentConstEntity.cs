using System;
using UnityEngine;

namespace Common.Equipments
{
    [Serializable]
    public class EquipmentConstEntity
    {
        [SerializeField] private DataIndex dataIndex;
        [SerializeField] private Spec constSpec;
        [SerializeField] private Sprite icon;
        [SerializeField] private string itemName;
        [SerializeField] private CombatClassType availableClassType;
        
        public DataIndex DataIndex => dataIndex;
        public Spec ConstSpec => constSpec;
        public Sprite Icon => icon;
        public string ItemName => itemName;
        public CombatClassType AvailableClassType => availableClassType;
        public EquipType EquipType => (EquipType)dataIndex.GetCategory();
        

        public void SetEntity(DataIndex dataIndex)
        {
            this.dataIndex = dataIndex;
            itemName = DataIndex.ToString().DivideWords();
            icon = Database.EquipmentSpriteData.Get(DataIndex);
            availableClassType = CombatClassType.All; // TEMP
        }
    }
}
