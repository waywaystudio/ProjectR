using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Common.Characters
{
    [Serializable]
    public class CharacterConstEntity
    {
        [SerializeField] private CharacterMask classType;
        [SerializeField] private string characterName;
        [SerializeField] private StatSpec defaultStatSpec;
        [SerializeField] private List<DataIndex> defaultSkillList;

        private const string ConstStatKey = "ConstStatKey"; 
        
        public CharacterMask ClassType => classType;
        public string CharacterName => characterName;
        public StatSpec DefaultStatSpec => defaultStatSpec;
        public List<DataIndex> DefaultSkillList => defaultSkillList;


#if UNITY_EDITOR
        public void EditorSetUpByDataIndex(DataIndex dataIndex)
        {
            if (!Verify.IsNotDefault(dataIndex, "Require Character DataIndex for import data")) 
                return;

            defaultStatSpec.Clear();
            defaultSkillList.Clear();

            switch (dataIndex.GetCategory())
            {
                case DataIndex.Venturer: LoadAdventurerData(dataIndex); break;
                case DataIndex.Villain:     LoadMonsterData(dataIndex); break;
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
            

            classType     = classData.BaseRole.ToEnum<CharacterMask>();
            characterName = classData.CharacterName;
            
            // defaultSpec
            defaultStatSpec.Add(StatType.Power, ConstStatKey, classData.BasePower);
            defaultStatSpec.Add(StatType.MinDamage, ConstStatKey, classData.DefaultDamage);
            defaultStatSpec.Add(StatType.MaxDamage, ConstStatKey, classData.DefaultDamage);
            defaultStatSpec.Add(StatType.Health, ConstStatKey, classData.Health);
            defaultStatSpec.Add(StatType.CriticalChance, ConstStatKey, classData.Critical);
            defaultStatSpec.Add(StatType.Haste, ConstStatKey, classData.Haste);
            defaultStatSpec.Add(StatType.Armor, ConstStatKey, classData.Armor);
            defaultStatSpec.Add(StatType.MaxResource, ConstStatKey, classData.MaxResource);
            defaultStatSpec.Add(StatType.MoveSpeed, ConstStatKey, classData.MoveSpeed);
            
            // defaultSkills
            classData.DefaultSkills.ForEach(skill => defaultSkillList.Add((DataIndex)skill));
        }
        private void LoadMonsterData(DataIndex dataIndex)
        {
            var monsterData = Database.BossSheetData(dataIndex);

            if (!Verify.IsNotNull(monsterData, $"Not Exist {dataIndex} in BossData")) return;

            classType     = CharacterMask.Villain;
            characterName = monsterData.FullName;
            
            // defaultSpec
            defaultStatSpec.Add(StatType.Power, ConstStatKey, monsterData.Power);
            defaultStatSpec.Add(StatType.MinDamage, ConstStatKey, monsterData.DefaultDamage);
            defaultStatSpec.Add(StatType.MaxDamage, ConstStatKey, monsterData.DefaultDamage);
            defaultStatSpec.Add(StatType.Health, ConstStatKey, monsterData.Health);
            defaultStatSpec.Add(StatType.CriticalChance, ConstStatKey, monsterData.Critical);
            defaultStatSpec.Add(StatType.Haste, ConstStatKey, monsterData.Haste);
            defaultStatSpec.Add(StatType.Armor, ConstStatKey, monsterData.Armor);
            defaultStatSpec.Add(StatType.MaxResource, ConstStatKey, monsterData.MaxResource);
            defaultStatSpec.Add(StatType.MoveSpeed, ConstStatKey, monsterData.MoveSpeed);
            
            // defaultSkills
            monsterData.DefaultSkills.ForEach(skill => defaultSkillList.Add((DataIndex)skill));
        }
#endif
    }
}
