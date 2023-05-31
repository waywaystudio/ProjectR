using System;
using System.Collections.Generic;
using Common.Equipments;
using Serialization;
using UnityEngine;

namespace Common.Characters
{
    [Serializable]
    public class CharacterEquipmentEntity
    {
        [SerializeField] private List<DataIndex> initialEquipmentIndexList;

        public Dictionary<EquipmentSlotType, EquipmentEntity> EquipmentTable { get; set; }
        public StatTable StatTable { get; set; } = new();
        public EthosTable EthosTable { get; set; } = new();

        public void Save(string providerName)
        {
            var serializeKey = $"{providerName}.EquipmentEntity";
            
            SaveManager.Save(serializeKey, EquipmentTable);
        }

        public void Load(string providerName)
        {
            var serializeKey = $"{providerName}.EquipmentEntity";
            
            EquipmentTable = SaveManager.Load<Dictionary<EquipmentSlotType, EquipmentEntity>>(serializeKey);

            if (EquipmentTable.IsNullOrEmpty())
            {
                EquipInitialEquipments();
            }
            
            EquipmentTable.Values.ForEach(equipment =>
            {
                equipment.Load(equipment.DataIndex);
                
                StatTable.Add(equipment.ConstStatSpec);
                // EthosTable.Add(equipment.RelicEthosSpec);
            });
        }

        public EquipmentEntity GetEquipment(EquipmentSlotType slotIndex)
        {
            if (!EquipmentTable.TryGetValue(slotIndex, out var result))
            {
                Debug.LogWarning($"Not Exist {slotIndex} in EquipmentTable");
                return null;
            }

            return result;
        }
        

        private void EquipInitialEquipments()
        {
            EquipmentTable = new Dictionary<EquipmentSlotType, EquipmentEntity>();
            
            initialEquipmentIndexList.ForEach(initialEquipmentIndex =>
            {
                var instance       = new EquipmentEntity(initialEquipmentIndex);
                var equipSlotIndex = initialEquipmentIndex.GetCategory();

                EquipmentTable.Add((EquipmentSlotType)equipSlotIndex, instance);
            });
        }
        

#if UNITY_EDITOR
        public void EditorSetUpByDataIndex(DataIndex dataIndex)
        {
            initialEquipmentIndexList.Clear();
            
            switch (dataIndex.GetCategory())
            {
                case DataIndex.Venturer: LoadAdventurerData(dataIndex); break;
                case DataIndex.Villain:  LoadMonsterData(dataIndex); break;
                default:
                {
                    Debug.LogWarning($"DataIndex Error. Must be CombatClass or Boss. Input Category:{dataIndex.GetCategory()}");
                    return;
                } 
            }
        }
        
        private void LoadAdventurerData(DataIndex dataIndex)
        {
            var classData = Database.CombatClassSheetData(dataIndex);

            if (!Verify.IsNotNull(classData, $"Not Exist {dataIndex} in AdventurerData")) return;

            classData.InitialEquipments.ForEach(equipment =>
            {
                initialEquipmentIndexList.Add((DataIndex)equipment);
            });
        }
        private void LoadMonsterData(DataIndex dataIndex)
        {
            var monsterData = Database.BossSheetData(dataIndex);

            if (!Verify.IsNotNull(monsterData, $"Not Exist {dataIndex} in BossData"))
            {
                
            }
            
            // TODO. 추후 기획에 Monster에 초기장비가 있다면 추가, 없다면 삭제.
            // monsterData.InitialEquipments.ForEach(equipment => initialEquipmentIndexList.Add((DataIndex)equipment));
        }
#endif
    }
}
