using System;
using UnityEngine;

namespace Common.Skills
{
    [Serializable]
    public class SkillCastTimer : CastTimer
    {
        [SerializeField] protected SectionType callbackSection;
        
        public SectionType CallbackSection => callbackSection;
        
        public void Initialize(SkillComponent skill)
        {
            skill.SequenceBuilder
                 .Add(SectionType.Active, "SkillCasting", () => Play(skill.CastWeightTime, CallbackSection.GetInvokeAction(skill)))
                 .Add(SectionType.End, "StopCastTimer", Stop);
        }


#if UNITY_EDITOR
        public void SetUpFromSkill(DataIndex dataIndex)
        {
            var skillData = Database.SkillSheetData(dataIndex);
            
            castingTime     = skillData.CastTime;
            callbackSection = skillData.CastCallback.ToEnum<SectionType>();
        }
#endif
    }
}
