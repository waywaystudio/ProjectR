using System;
using UnityEngine;

namespace Common.Skills
{
    [Serializable]
    public class SkillCoolTimer : CoolTimer
    {
        [SerializeField] protected Section invokeSection;
        
        public Section InvokeSection => invokeSection;
        
        public void Initialize(SkillComponent skill)
        {
            if (InvokeSection == Section.None) return;
            
            skill.Builder
                 .AddCondition("IsCoolTimeReady", () => IsReady)
                 .Add(InvokeSection, "ActiveCoolTime", () => Play(skill.CoolingWeight));
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
