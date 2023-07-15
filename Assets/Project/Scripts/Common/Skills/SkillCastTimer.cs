using System;
using UnityEngine;

namespace Common.Skills
{
    [Serializable]
    public class SkillCastTimer : TimeTrigger
    {
        [SerializeField] protected Section callbackSection;

        public float OriginalCastDuration
        {
            get => duration; 
            set => duration = value;
        }

        public void Initialize(SkillComponent skill)
        {
            if (duration == 0f) return;

            InitializeTrigger()
                .SetWeight(() => CombatFormula.HasteValue(skill.Haste))
                .SetCallback(callbackSection.GetCombatInvoker(skill.Invoker));

            var hasRelease = skill.SkillType is SkillType.Charging or SkillType.Holding;
            var builder = new CombatSequenceBuilder(skill.Sequence);

            builder
                .Add(Section.Active, "SkillCasting",Play)
                .AddIf(callbackSection != Section.None, callbackSection, "StopCasting", Stop)
                .AddIf(hasRelease, Section.Release, "ReleaseAction", () => InvokeCallbackSection(skill.Invoker))
                .Add(Section.End, "StopCasting",  Stop)
                ;
        }


        private void InvokeCallbackSection(CombatSequenceInvoker invoker)
        {
            if (!invoker.IsActive) return;
            
            callbackSection.GetCombatInvoker(invoker)?.Invoke();
        }


#if UNITY_EDITOR
        public void SetUpFromSkill(DataIndex dataIndex)
        {
            var skillData = Database.SkillSheetData(dataIndex);

            isIncrease      = true;
            duration        = skillData.CastTime;
            callbackSection = skillData.CastCallback.ToEnum<Section>();
        }
#endif
    }
}
