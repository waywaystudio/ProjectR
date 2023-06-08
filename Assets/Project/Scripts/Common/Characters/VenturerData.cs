using UnityEngine;

namespace Common.Characters
{
    public class VenturerData : CharacterData
    {
        [SerializeField] private CharacterEquipmentEntity equipmentEntity;
        
        public VenturerType VenturerType => (VenturerType)characterIndex;
        public EthosTable StaticEthosTable => equipmentEntity.EthosTable;
        
        public int GetEthosValue(EthosType type) => StaticEthosTable.GetEthosValue(type);
        public IEquipment GetWeapon() => equipmentEntity.GetWeapon();
        public IEquipment GetAmor() => equipmentEntity.GetArmor();
        
        public override void Save()
        {
            base.Save();
            
            equipmentEntity.Save(characterIndex.ToString());
        }

        public override void Load()
        {
            base.Load();
            
            equipmentEntity.Load(characterIndex.ToString());
            StaticStatTable.RegisterTable(equipmentEntity.StatTable);
        }
        
        
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();

            equipmentEntity.EditorSetUpByDataIndex(characterIndex);
        }
#endif
    }
}
