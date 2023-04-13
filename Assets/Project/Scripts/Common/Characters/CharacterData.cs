using System.Collections.Generic;
using Common.Equipments;
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

        public CombatClassType ClassType => classType;
        public DataIndex DataIndex => dataIndex;
        public Spec ClassSpec => classSpec;

        [Sirenix.OdinInspector.ShowInInspector]
        public Dictionary<EquipSlotIndex, EquipmentInfo> Table { get; private set; } = new();

        private string SerializeKey => $"{characterName}'s Equipments";
        

        public float GetStat(StatType type)
        {
            var equipmentStat = 0f;
            
            Table.ForEach(table =>
            {
                if (table.Value == null) return;

                equipmentStat += table.Value.Spec.GetStatValue(type);
            });

            return classSpec.GetStatValue(type) + equipmentStat;
        }

        public void Save()
        {
            SaveManager.Save(SerializeKey, Table);
        }

        public void Load()
        {
            Table.Clear();
            
            var tableData = SaveManager.Load<Dictionary<EquipSlotIndex, EquipmentInfo>>(SerializeKey);

            if (tableData.IsNullOrEmpty()) return;
            
            Table = tableData;
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

                    classSpec.Add(StatType.CriticalChance, StatApplyType.Plus, classData.Critical);
                    classSpec.Add(StatType.Haste,          StatApplyType.Plus, classData.Haste);
                    classSpec.Add(StatType.Armor,          StatApplyType.Plus, classData.Armor);
                    classSpec.Add(StatType.MaxHp,          StatApplyType.Plus, classData.MaxHp);
                    classSpec.Add(StatType.MaxResource,    StatApplyType.Plus, classData.MaxResource);
                    classSpec.Add(StatType.MoveSpeed,      StatApplyType.Plus, classData.MoveSpeed);
                    break;
                }
                case DataIndex.Boss:
                {
                    var bossData = Database.BossSheetData(dataIndex);

                    classSpec.Add(StatType.CriticalChance, StatApplyType.Plus, bossData.Critical);
                    classSpec.Add(StatType.Haste,          StatApplyType.Plus, bossData.Haste);
                    classSpec.Add(StatType.Armor,          StatApplyType.Plus, bossData.Armor);
                    classSpec.Add(StatType.MaxHp,          StatApplyType.Plus, bossData.MaxHp);
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
