using System;
using UnityEngine;

namespace Common.Skills
{
    [Serializable]
    public class SkillCost
    {
        [SerializeField] private float cost;
        [SerializeField] private SectionType paySection;

        public float Value => cost;

        public void Initialize(SkillComponent skill)
        {
            var resourceRef = skill.Cb.DynamicStatEntry.Resource;
            
            skill.SequenceBuilder
                 .AddCondition("CheckCost", () => resourceRef.Value >= Value)
                 .Add(paySection, "payCost", () => resourceRef.Value -= Value);
        }


#if UNITY_EDITOR
        public void SetUpFromSkill(DataIndex dataIndex)
        {
            var skillData = Database.SkillSheetData(dataIndex);
            
            cost = skillData.Cost;
        }
#endif
    }
}
