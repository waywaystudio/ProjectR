using System;
using UnityEngine;

namespace Common.Skills
{
    [Serializable]
    public class SkillCost
    {
        [SerializeField] private float cost;
        [SerializeField] private bool requireTaker;
        [SerializeField] private Section paySection;

        public float Value => cost;

        public void Initialize(SkillComponent skill)
        {
            if (paySection == Section.None) return;
            
            var resourceRef = skill.Cb.Resource;
            
            skill.Builder
                 .AddCondition("CheckCost", () => resourceRef.Value >= Value)
                 .AddIf(!requireTaker, paySection, "payCost", () => PayCost(resourceRef));
        }

        public void PayCost(ResourceValue resourceValue)
        {
            resourceValue.Value -= Value;
        }


#if UNITY_EDITOR
        public void SetUpFromSkill(DataIndex dataIndex)
        {
            var skillData = Database.SkillSheetData(dataIndex);
            
            cost         = skillData.Cost;
            requireTaker = skillData.RequireTaker;
            paySection   = skillData.PaySection.ToEnum<Section>();
        }
#endif
    }
}
