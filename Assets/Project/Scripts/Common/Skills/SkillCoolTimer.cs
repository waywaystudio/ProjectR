using System;
using UnityEngine;

namespace Common.Skills
{
    [Serializable]
    public class SkillCoolTimer : CoolTimer
    {
        [SerializeField] private Section invokeSection;
        
        public void Initialize(SkillComponent skill)
        {
            if (invokeSection == Section.None) return;
            
            skill.Builder
                 .AddCondition("IsCoolTimeReady", () => IsReady)
                 .Add(invokeSection, "ActiveCoolTime", () => Play(skill.CoolingWeight));
        }
        

#if UNITY_EDITOR
        public void SetUpAsSkill(DataIndex dataIndex)
        {
            var skillData = Database.SkillSheetData(dataIndex);
            
            coolTime      = skillData.CoolTime;
            invokeSection = skillData.CoolTimeInvoker.ToEnum<Section>();
        }
#endif
    }
}
