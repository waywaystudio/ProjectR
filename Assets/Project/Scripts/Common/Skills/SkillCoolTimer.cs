using System;
using UnityEngine;

namespace Common.Skills
{
    [Serializable]
    public class SkillCoolTimer : CoolTimer
    {
        [SerializeField] protected SectionType invokeSection;
        
        public SectionType InvokeSection => invokeSection;
        
        public void Initialize(SkillComponent skill)
        {
            if (InvokeSection == SectionType.None) return;
            
            skill.SequenceBuilder
                 .AddCondition("IsCoolTimeReady", () => IsReady)
                 .Add(InvokeSection, "ActiveCoolTime", () => Play(skill.CoolWeightTime));
        }
        

#if UNITY_EDITOR
        public void SetUpAsSkill(DataIndex dataIndex)
        {
            var skillData = Database.SkillSheetData(dataIndex);
            
            coolTime      = skillData.CoolTime;
            invokeSection = skillData.CoolTimeInvoker.ToEnum<SectionType>();
        }
#endif
    }
}
