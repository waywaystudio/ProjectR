using System;
using UnityEngine;

namespace Common.Skills
{
    [Serializable]
    public class CombatCastTimer : TimeTrigger
    {
        [SerializeField] protected Section callbackSection;
        
        public Section CallbackSection => callbackSection;
        public float OriginalDuration
        {
            get => duration; 
            set => duration = value;
        }

        public void Initialize(ICombatObject combatObject)
        {
            if (duration == 0f) return;

            InitializeTrigger()
                .SetWeight(() => CombatFormula.HasteValue(combatObject.Haste))
                .SetCallback(callbackSection.GetCombatInvoker(combatObject.Invoker));

            var builder = new CombatSequenceBuilder(combatObject.Sequence);
            builder
                .Add(Section.Active, "SkillCasting",Play)
                .AddIf(callbackSection != Section.None, callbackSection, "StopCasting", Stop)
                .Add(Section.End, "StopCasting",  Stop)
                ;
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
