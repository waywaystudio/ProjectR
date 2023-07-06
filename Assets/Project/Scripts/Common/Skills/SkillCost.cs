using System;
using UnityEngine;

namespace Common.Skills
{
    [Serializable]
    public class SkillCost
    {
        [SerializeField] private float cost;
        [SerializeField] private Section paySection;

        public float Value => cost;
        public ConditionTable PayCondition { get; set; } = new();

        public void Initialize(SkillComponent skill)
        {
            if (paySection == Section.None) return;
            
            var resourceRef = skill.Cb.Resource;
            
            skill.Builder
                 .AddCondition("CheckCost", () => resourceRef.Value >= Value)
                 .Add(paySection, "payCost", () => PayCost(resourceRef));
        }


        private void PayCost(ResourceValue resourceValue)
        {
            if (PayCondition.IsAllTrue)
            {
                resourceValue.Value -= Value;
            }
        }


#if UNITY_EDITOR
        public void SetUpFromSkill(DataIndex dataIndex)
        {
            var skillData = Database.SkillSheetData(dataIndex);
            
            cost       = skillData.Cost;
            paySection = skillData.PaySection.ToEnum<Section>();
        }
#endif
    }
}
