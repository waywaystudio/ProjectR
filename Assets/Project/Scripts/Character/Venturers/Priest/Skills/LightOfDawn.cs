using Common;
using Common.Skills;

namespace Character.Venturers.Priest.Skills
{
    public class LightOfDawn : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();

            Builder
                .Add(SectionType.Execute, "PlayCastCompleteAnimation", PlayCastCompleteAnimation)
                .Add(SectionType.Execute, "ExecuteLightOfDawn", ExecuteLightOfDawn);
        }
        
        
        private void PlayCastCompleteAnimation()
        {
            Cb.Animating.PlayOnce("CastHoldFire", 1f + Haste, Invoker.Complete);
        }

        private void ExecuteLightOfDawn()
        {
            var onRapture = Provider.StatusEffectTable.ContainsKey(DataIndex.LightWeaverStatusEffect);
            
            detector.GetTakers()?.ForEach(taker =>
            {
                executor.ToTaker(taker);

                if (onRapture)
                {
                    executor.ToTaker(taker);
                }
            });
        }
    }
}
