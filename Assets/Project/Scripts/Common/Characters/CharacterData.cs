using System;
using System.Collections.Generic;
using Common.Equipments;
using Serialization;
using UnityEngine;

namespace Common.Characters
{
    public class CharacterData : ScriptableObject, ISavable, IEditable
    {
        [SerializeField] private CharacterConstEntity constEntity;

        [Sirenix.OdinInspector.ShowInInspector]
        public StatTable StaticSpecTable { get; } = new();
        
        public DataIndex DataIndex => constEntity.DataIndex;
        public CombatClassType ClassType => constEntity.ClassType;
        public IEnumerable<DataIndex> DefaultSkillList => constEntity.DefaultSkillList;
        public Spec ClassSpec => constEntity.DefaultSpec;

        [Sirenix.OdinInspector.ShowInInspector]
        public Dictionary<EquipSlotIndex, EquipmentInfo> EquipmentTable { get; private set; } = new();


        private string SerializeKey => $"{constEntity.CharacterName}'s Equipments";
        public float GetStatValue(StatType type)
        {
            // if (!Verify.IsNotNull(value)) return;
            
            var equipmentStat = 0f;
            
            EquipmentTable.ForEach(table =>
            {
                if (table.Value == null) return;

                equipmentStat += table.Value.ConstSpec.GetStatValue(type);
            });

            return ClassSpec.GetStatValue(type) + equipmentStat;
        }

        public void AddEquipment(Equipment equipment, out EquipmentInfo disarmed)
        {
            var targetSlot = FindSlot(equipment);

            EquipmentTable.TryGetValue(targetSlot, out disarmed);
            // EquipmentTable[targetSlot] = equipment.Info;
        }

        public void UpdateTable()
        {
            // StaticSpecTable.Add("ClassSpec", classSpec);
            StaticSpecTable.Add(ClassSpec);
        }

        public void Save()
        {
            SaveManager.Save(SerializeKey, EquipmentTable);
        }

        public void Load()
        {
            EquipmentTable.Clear();
            
            var tableData = SaveManager.Load<Dictionary<EquipSlotIndex, EquipmentInfo>>(SerializeKey);

            UpdateTable();

            if (tableData.IsNullOrEmpty()) return;

            // tableData.Values.ForEach(equipInfo => StaticSpecTable.Add(equipInfo.EquipType.ToString(), equipInfo.Spec));
            tableData.Values.ForEach(equipInfo => StaticSpecTable.Add(equipInfo.ConstSpec));
            EquipmentTable = tableData;
        }
        

        private EquipSlotIndex FindSlot(Equipment equipment)
        {
            return equipment.EquipType switch
            {
                EquipType.Weapon  => EquipSlotIndex.Weapon,
                EquipType.Head    => EquipSlotIndex.Head,
                EquipType.Top     => EquipSlotIndex.Top,
                EquipType.Bottom  => EquipSlotIndex.Bottom,
                EquipType.Trinket => GetTrinketSlot(),
                _                 => throw new ArgumentOutOfRangeException()
            };
        }

        private EquipSlotIndex GetTrinketSlot()
        {
            if (!EquipmentTable.TryGetValue(EquipSlotIndex.Trinket1, out var value1)) 
                return EquipSlotIndex.Trinket1;
            
            if (value1 == null)
            {
                return EquipSlotIndex.Trinket1;
            }

            if (EquipmentTable.TryGetValue(EquipSlotIndex.Trinket2, out var value2))
            {
                if (value2 == null) return EquipSlotIndex.Trinket2;
            }
            else
            {
                return EquipSlotIndex.Trinket1;
            }

            return EquipSlotIndex.Trinket1;
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            
        }
#endif
    }
}
