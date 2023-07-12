using System;
using UnityEngine;

namespace Common.Skills
{
    [Serializable]
    public class SkillCastTimer : CastTimer
    {
        [SerializeField] protected Section callbackSection;
        
        public Section CallbackSection => callbackSection;
        
        public void Initialize(SkillComponent skill)
        {
            skill.Builder
                 .Add(Section.Active, "SkillCasting", () => Play(skill.CastingWeight, CallbackSection.GetCombatInvoker(skill.Invoker)))
                 .Add(Section.End, "StopCastTimer",  Stop);

            if (callbackSection != Section.None)
            {
                 skill.Builder
                      .Add(callbackSection, "StopCastTimer", Stop);
                 
            }
        }


#if UNITY_EDITOR
        public void SetUpFromSkill(DataIndex dataIndex)
        {
            var skillData = Database.SkillSheetData(dataIndex);
            
            castingTime     = skillData.CastTime;
            callbackSection = skillData.CastCallback.ToEnum<Section>();
        }
#endif
    }
}
