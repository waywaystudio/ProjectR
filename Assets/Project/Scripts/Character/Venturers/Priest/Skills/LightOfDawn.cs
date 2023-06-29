using Common;
using Common.Skills;

namespace Character.Venturers.Priest.Skills
{
    public class LightOfDawn : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();

            SequenceBuilder
                .Add(SectionType.Execute, "PlayCastCompleteAnimation", PlayCastCompleteAnimation)
                .Add(SectionType.Execute, "ExecuteLightOfDawn", ExecuteLightOfDawn);
        }
        
        
        private void PlayCastCompleteAnimation()
        {
            Cb.Animating.PlayOnce("CastHoldFire", 1f + Haste, SkillInvoker.Complete);
        }

        private void ExecuteLightOfDawn()
        {
            var onRapture = Provider.DynamicStatEntry.StatusEffectTable.ContainsKey(DataIndex.LightWeaverStatusEffect);
            
            detector.GetTakers()?.ForEach(taker =>
            {
                executor.Execute(taker);

                if (onRapture)
                {
                    executor.Execute(taker);
                }
            });
        }
    }
}
