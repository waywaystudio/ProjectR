using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Characters
{
    [Serializable]
    public class CharacterConstEntity : IEditable
    {
        [SerializeField] private DataIndex dataIndex;
        [SerializeField] private CombatClassType classType;
        [SerializeField] private string characterName;
        [SerializeField] private Spec defaultSpec;
        [SerializeField] private List<DataIndex> defaultSkillList;
        [SerializeField] private List<DataIndex> initialEquipmentList;
        
        public CombatClassType ClassType => classType;
        public DataIndex DataIndex => dataIndex;
        public string CharacterName => characterName;
        public Spec DefaultSpec => defaultSpec;
        public List<DataIndex> DefaultSkillList => defaultSkillList;
        public List<DataIndex> InitialEquipmentList => initialEquipmentList;


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            if (dataIndex == DataIndex.None)
            {
                Debug.LogError("Require Character DataIndex for import data");
                return;
            }
            
            switch (dataIndex.GetCategory())
            {
                case DataIndex.CombatClass: LoadAdventurerData(dataIndex); break;
                case DataIndex.Boss: LoadMonsterData(dataIndex); break;
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

            if (classData == null)
            {
                Debug.LogError($"Not Exist {dataIndex} in AdventurerData");
                return;
            }

            classType     = classData.BaseRole.ToEnum<CombatClassType>();
            characterName = classData.CharacterName;
            
            // defaultSpec
            defaultSpec.Add(StatType.MinDamage, StatApplyType.Plus, classData.DefaultDamage);
            defaultSpec.Add(StatType.MaxDamage, StatApplyType.Plus, classData.DefaultDamage);
            defaultSpec.Add(StatType.Health, StatApplyType.Plus, classData.Health);
            defaultSpec.Add(StatType.CriticalChance, StatApplyType.Plus, classData.Critical);
            defaultSpec.Add(StatType.Haste,          StatApplyType.Plus, classData.Haste);
            defaultSpec.Add(StatType.Armor,          StatApplyType.Plus, classData.Armor);
            defaultSpec.Add(StatType.MaxResource,    StatApplyType.Plus, classData.MaxResource);
            defaultSpec.Add(StatType.MoveSpeed,      StatApplyType.Plus, classData.MoveSpeed);
            
            // defaultSkills
            defaultSkillList.Clear();
            classData.DefaultSkills.ForEach(skill => defaultSkillList.Add((DataIndex)skill));
            
            // defaultEquipments
            initialEquipmentList.Clear();
            classData.InitialEquipments.ForEach(equipment => initialEquipmentList.Add((DataIndex)equipment));
        }
        private void LoadMonsterData(DataIndex dataIndex)
        {
            var monsterData = Database.BossSheetData(dataIndex);

            if (monsterData == null)
            {
                Debug.LogError($"Not Exist {dataIndex} in BossData");
                return;
            }

            classType     = CombatClassType.Boss;
            characterName = monsterData.FullName;
            
            // defaultSpec
            defaultSpec.Add(StatType.MinDamage, StatApplyType.Plus, monsterData.DefaultDamage);
            defaultSpec.Add(StatType.MaxDamage, StatApplyType.Plus, monsterData.DefaultDamage);
            defaultSpec.Add(StatType.Health, StatApplyType.Plus, monsterData.Health);
            defaultSpec.Add(StatType.CriticalChance, StatApplyType.Plus, monsterData.Critical);
            defaultSpec.Add(StatType.Haste,          StatApplyType.Plus, monsterData.Haste);
            defaultSpec.Add(StatType.Armor,          StatApplyType.Plus, monsterData.Armor);
            defaultSpec.Add(StatType.MaxResource,    StatApplyType.Plus, monsterData.MaxResource);
            defaultSpec.Add(StatType.MoveSpeed,      StatApplyType.Plus, monsterData.MoveSpeed);
            
            // defaultSkills
            defaultSkillList.Clear();
            monsterData.DefaultSkills.ForEach(skill => defaultSkillList.Add((DataIndex)skill));
        }
#endif
    }
}
