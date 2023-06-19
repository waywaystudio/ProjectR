using System;

namespace Common.Skills
{
    [Serializable]
    public class SkillCastTimer : CastTimer
    {
        public void Initialize(SkillComponent skill)
        {
            skill.SequenceBuilder
                 .Add(SectionType.Active, "SkillCasting", () => Play(skill.CastWeightTime, CallbackSection.GetInvokeAction(skill)))
                 .Add(SectionType.End, "StopCastTimer", Stop);
        }


        public void SetUpFromSkill(DataIndex dataIndex)
        {
            var skillData = Database.SkillSheetData(dataIndex);
            
            castingTime     = skillData.CastTime;
            callbackSection = skillData.CastCallback.ToEnum<SectionType>();
        }
    }
}
