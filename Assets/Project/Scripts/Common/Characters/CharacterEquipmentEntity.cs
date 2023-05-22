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

        public Dictionary<EquipSlotIndex, EquipmentEntity> EquipmentEntities { get; set; }
        public StatTable EquipmentsStatTable { get; set; } = new();

        public void Equip(EquipSlotIndex equipIndex, EquipmentEntity entity)
        {
            if (!EquipmentEntities.TryAdd(equipIndex, entity))
            {
                EquipmentEntities[equipIndex] = entity;
            }
        }

        public void Save(string providerName)
        {
            var serializeKey = $"{providerName}.EquipmentEntity";
            
            SaveManager.Save(serializeKey, EquipmentEntities);
        }

        public void Load(string providerName)
        {
            var serializeKey = $"{providerName}.EquipmentEntity";
            
            EquipmentEntities = SaveManager.Load<Dictionary<EquipSlotIndex, EquipmentEntity>>(serializeKey);

            if (EquipmentEntities.IsNullOrEmpty())
            {
                EquipStartEquipments();
            }
            
            EquipmentEntities.Values.ForEach(equipment =>
            {
                equipment.Load(equipment.DataIndex);
                EquipmentsStatTable.Add(equipment.Spec);
            });
        }


        private EquipmentEntity GetEquipment(EquipSlotIndex slotIndex)
        {
            if (!EquipmentEntities.TryGetValue(slotIndex, out var result))
            {
                Debug.LogWarning($"Not Exist {slotIndex} in EquipmentTable");
                return null;
            }

            return result;
        }

        private void EquipStartEquipments()
        {
            EquipmentEntities = new Dictionary<EquipSlotIndex, EquipmentEntity>();
            
            initialEquipmentIndexList.ForEach(initialEquipmentIndex =>
            {
                var instance       = new EquipmentEntity(initialEquipmentIndex);
                var equipSlotIndex = initialEquipmentIndex.GetCategory();

                EquipmentEntities.Add((EquipSlotIndex)equipSlotIndex, instance);
            });
        }
        

#if UNITY_EDITOR
        public void EditorSetUpByDataIndex(DataIndex dataIndex)
        {
            initialEquipmentIndexList.Clear();
            
            switch (dataIndex.GetCategory())
            {
                case DataIndex.CombatClass: LoadAdventurerData(dataIndex); break;
                case DataIndex.Boss:        LoadMonsterData(dataIndex); break;
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

            classData.InitialEquipments.ForEach(equipment => initialEquipmentIndexList.Add((DataIndex)equipment));
        }
        private void LoadMonsterData(DataIndex dataIndex)
        {
            var monsterData = Database.BossSheetData(dataIndex);

            if (!Verify.IsNotNull(monsterData, $"Not Exist {dataIndex} in BossData")) return;
            
            // TODO. 추후 기획에 Monster에 초기장비가 있다면 추가, 없다면 삭제.
            // monsterData.InitialEquipments.ForEach(equipment => initialEquipmentIndexList.Add((DataIndex)equipment));
        }
#endif
    }
}
