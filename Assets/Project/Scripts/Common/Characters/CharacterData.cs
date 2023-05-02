using System;
using System.Collections.Generic;
using Common.Equipments;
using Common.Skills;
using Serialization;
using UnityEngine;

namespace Common.Characters
{
    public class CharacterData : ScriptableObject, ISavable, IEditable
    {
        [SerializeField] private CombatClassType classType;
        [SerializeField] private DataIndex dataIndex;
        [SerializeField] private string characterName;
        [SerializeField] private Spec classSpec;
        [SerializeField] private List<SkillComponent> skillList;

        public CombatClassType ClassType => classType;
        public DataIndex DataIndex => dataIndex;
        
        [Sirenix.OdinInspector.ShowInInspector]
        public StatTable StaticSpecTable { get; } = new();
        public List<SkillComponent> SkillList => skillList;

        [Sirenix.OdinInspector.ShowInInspector]
        public Dictionary<EquipSlotIndex, EquipmentInfo> Table { get; private set; } = new();

        private string SerializeKey => $"{characterName}'s Equipments";
        

        public float GetStatValue(StatType type)
        {
            var equipmentStat = 0f;
            
            Table.ForEach(table =>
            {
                if (table.Value == null) return;

                equipmentStat += table.Value.Spec.GetStatValue(type);
            });

            return classSpec.GetStatValue(type) + equipmentStat;
        }

        public void AddEquipment(Equipment equipment, out EquipmentInfo disarmed)
        {
            var targetSlot = FindSlot(equipment);

            Table.TryGetValue(targetSlot, out disarmed);
            Table[targetSlot] = equipment.Info;
        }

        public void UpdateTable()
        {
            StaticSpecTable.Add("ClassSpec", classSpec);
        }

        public void Save()
        {
            SaveManager.Save(SerializeKey, Table);
        }

        public void Load()
        {
            Table.Clear();
            
            var tableData = SaveManager.Load<Dictionary<EquipSlotIndex, EquipmentInfo>>(SerializeKey);

            UpdateTable();

            if (tableData.IsNullOrEmpty()) return;

            tableData.Values.ForEach(equipInfo => StaticSpecTable.Add(equipInfo.EquipType.ToString(), equipInfo.Spec));
            Table = tableData;
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
            if (!Table.TryGetValue(EquipSlotIndex.Trinket1, out var value1)) 
                return EquipSlotIndex.Trinket1;
            
            if (value1 == null)
            {
                return EquipSlotIndex.Trinket1;
            }

            if (Table.TryGetValue(EquipSlotIndex.Trinket2, out var value2))
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
            classSpec.Clear();

            switch (dataIndex.GetCategory())
            {
                case DataIndex.CombatClass:
                {
                    var classData = Database.CombatClassSheetData(dataIndex);

                    classSpec.Add(StatType.MinDamage, StatApplyType.Plus, classData.DefaultDamage);
                    classSpec.Add(StatType.MaxDamage, StatApplyType.Plus, classData.DefaultDamage);
                    classSpec.Add(StatType.Health, StatApplyType.Plus, classData.Health);
                    classSpec.Add(StatType.CriticalChance, StatApplyType.Plus, classData.Critical);
                    classSpec.Add(StatType.Haste,          StatApplyType.Plus, classData.Haste);
                    classSpec.Add(StatType.Armor,          StatApplyType.Plus, classData.Armor);
                    classSpec.Add(StatType.MaxResource,    StatApplyType.Plus, classData.MaxResource);
                    classSpec.Add(StatType.MoveSpeed,      StatApplyType.Plus, classData.MoveSpeed);
                    break;
                }
                case DataIndex.Boss:
                {
                    var bossData = Database.BossSheetData(dataIndex);

                    classSpec.Add(StatType.MinDamage, StatApplyType.Plus, bossData.DefaultDamage);
                    classSpec.Add(StatType.MaxDamage, StatApplyType.Plus, bossData.DefaultDamage);
                    classSpec.Add(StatType.Health, StatApplyType.Plus, bossData.Health);
                    classSpec.Add(StatType.CriticalChance, StatApplyType.Plus, bossData.Critical);
                    classSpec.Add(StatType.Haste,          StatApplyType.Plus, bossData.Haste);
                    classSpec.Add(StatType.Armor,          StatApplyType.Plus, bossData.Armor);
                    classSpec.Add(StatType.MaxResource,    StatApplyType.Plus, bossData.MaxResource);
                    classSpec.Add(StatType.MoveSpeed,      StatApplyType.Plus, bossData.MoveSpeed);
                    break;
                }
                default:
                {
                    Debug.LogWarning($"DataIndex Error. Must be CombatClass or Boss. Input Category:{dataIndex.GetCategory()}");
                    return;
                } 
            }
        }
#endif
    }
}
