using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Priest.Skills
{
    public class Rapture : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();

            SequenceBuilder
                .AddCondition("HasRaptureAlready", HasRaptureAlready)
                .Add(SectionType.Execute, "ExecuteHealingTouch",ExecuteRapture);
        }


        private bool HasRaptureAlready()
        {
            return !Provider.DynamicStatEntry.StatusEffectTable.ContainsKey(DataIndex.LightWeaverStatusEffect);
        }
        
        private void ExecuteRapture()
        {
            detector.GetTakers().ForEach(executor.Execute);
        }
    }
}
