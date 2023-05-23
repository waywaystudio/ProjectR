using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Characters
{
    [Serializable]
    public class CharacterConstEntity
    {
        [SerializeField] private CombatClassType classType;
        [SerializeField] private string characterName;
        [SerializeField] private Spec defaultSpec;
        [SerializeField] private List<DataIndex> defaultSkillList;

        private const string ConstStatKey = "ConstStatKey"; 
        
        public CombatClassType ClassType => classType;
        public string CharacterName => characterName;
        public Spec DefaultSpec => defaultSpec;
        public List<DataIndex> DefaultSkillList => defaultSkillList;


#if UNITY_EDITOR
        public void EditorSetUpByDataIndex(DataIndex dataIndex)
        {
            if (!Verify.IsNotDefault(dataIndex, "Require Character DataIndex for import data")) 
                return;

            defaultSpec.Clear();
            defaultSkillList.Clear();

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

            if (!Verify.IsNotNull(classData, $"Not Exist {dataIndex} in AdventurerData")) return;
            

            classType     = classData.BaseRole.ToEnum<CombatClassType>();
            characterName = classData.CharacterName;
            
            // defaultSpec
            defaultSpec.Add(StatType.Power, ConstStatKey, classData.BasePower);
            defaultSpec.Add(StatType.MinDamage, ConstStatKey, classData.DefaultDamage);
            defaultSpec.Add(StatType.MaxDamage, ConstStatKey, classData.DefaultDamage);
            defaultSpec.Add(StatType.Health, ConstStatKey, classData.Health);
            defaultSpec.Add(StatType.CriticalChance, ConstStatKey, classData.Critical);
            defaultSpec.Add(StatType.Haste, ConstStatKey, classData.Haste);
            defaultSpec.Add(StatType.Armor, ConstStatKey, classData.Armor);
            defaultSpec.Add(StatType.MaxResource, ConstStatKey, classData.MaxResource);
            defaultSpec.Add(StatType.MoveSpeed, ConstStatKey, classData.MoveSpeed);
            
            // defaultSkills
            classData.DefaultSkills.ForEach(skill => defaultSkillList.Add((DataIndex)skill));
        }
        private void LoadMonsterData(DataIndex dataIndex)
        {
            var monsterData = Database.BossSheetData(dataIndex);

            if (!Verify.IsNotNull(monsterData, $"Not Exist {dataIndex} in BossData")) return;

            classType     = CombatClassType.Boss;
            characterName = monsterData.FullName;
            
            // defaultSpec
            defaultSpec.Add(StatType.Power, ConstStatKey, monsterData.Power);
            defaultSpec.Add(StatType.MinDamage, ConstStatKey, monsterData.DefaultDamage);
            defaultSpec.Add(StatType.MaxDamage, ConstStatKey, monsterData.DefaultDamage);
            defaultSpec.Add(StatType.Health, ConstStatKey, monsterData.Health);
            defaultSpec.Add(StatType.CriticalChance, ConstStatKey, monsterData.Critical);
            defaultSpec.Add(StatType.Haste, ConstStatKey, monsterData.Haste);
            defaultSpec.Add(StatType.Armor, ConstStatKey, monsterData.Armor);
            defaultSpec.Add(StatType.MaxResource, ConstStatKey, monsterData.MaxResource);
            defaultSpec.Add(StatType.MoveSpeed, ConstStatKey, monsterData.MoveSpeed);
            
            // defaultSkills
            monsterData.DefaultSkills.ForEach(skill => defaultSkillList.Add((DataIndex)skill));
        }
#endif
    }
}
