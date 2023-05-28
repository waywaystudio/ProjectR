using System.Collections.Generic;
using Common.Equipments;
using Serialization;
using UnityEngine;

namespace Common.Characters
{
    /*
     * CharacterData 는 Prefab과 UI에서 사용하는 핵심 데이타.
     * 따라서 편의성 함수들이 존재 해야함.
     * */
    public class CharacterData : ScriptableObject, ISavable, IEditable
    {
        [SerializeField] private DataIndex characterIndex;
        [SerializeField] private CharacterConstEntity constEntity;
        [SerializeField] private CharacterEquipmentEntity equipmentEntity;

        public DataIndex DataIndex => characterIndex;
        public VenturerType VenturerType => (VenturerType)characterIndex;
        public CharacterMask ClassType => constEntity.ClassType;
        public IEnumerable<DataIndex> SkillList => constEntity.DefaultSkillList;
        public CharacterConstEntity ConstEntity => constEntity;
        public CharacterEquipmentEntity EquipmentEntity => equipmentEntity;
        public StatTable StaticStatTable { get; set; } = new();
        public EthosTable StaticEthosTable => equipmentEntity.EthosTable;

        public int GetEthosValue(EthosType type) => StaticEthosTable.GetEthosValue(type);
        public int GetRelicPieceCount(RelicType type) => EquipmentEntity.GetRelicPieceCount(type);
        public float GetStatValue(StatType type) => StaticStatTable.GetStatValue(type);
        public string GetStatTextValue(StatType type) => StaticStatTable.GetStatValue(type).ToStatUIValue(type);
        public EquipmentEntity GetEquipment(EquipSlotIndex slot) => EquipmentEntity.GetEquipment(slot);
        
        public void Save()
        {
            equipmentEntity.Save(characterIndex.ToString());
        }

        public void Load()
        {
            equipmentEntity.Load(characterIndex.ToString());
            
            StaticStatTable.Clear();
            StaticStatTable.Add(constEntity.DefaultStatSpec);
            StaticStatTable.RegisterTable(equipmentEntity.StatTable);
        }

        public void ChangeEquipmentRelic(EquipSlotIndex slotType, RelicType relicType)
        {
            equipmentEntity.ChangeRelic(slotType, relicType);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            if (!Verify.IsNotDefault(characterIndex, "", false))
            {
                var soObjectNameToDatIndex = name.Replace("Data", "").ConvertDataIndexStyle();
                if (!DataIndex.TryFindDataIndex(soObjectNameToDatIndex, out characterIndex)) return;
            }
            
            constEntity.EditorSetUpByDataIndex(characterIndex);
            equipmentEntity.EditorSetUpByDataIndex(characterIndex);
        }
#endif
    }
}
