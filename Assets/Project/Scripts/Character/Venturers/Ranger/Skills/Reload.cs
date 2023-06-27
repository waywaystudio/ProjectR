using Character.Venturers.Ranger.StatusEffects;
using Common.Skills;

namespace Character.Venturers.Ranger.Skills
{
    public class Reload : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();

            SequenceBuilder.Add(SectionType.Execute, "DamageByArrowsCount", CollectArrows);
        }


        private void CollectArrows()
        {
            var takers = detector.GetTakersInCircleRange(100f, 360f);
            
            takers?.ForEach(taker =>
            {
                if (!taker.DynamicStatEntry
                          .StatusEffectTable
                          .TryGetEffect<ArcaneArrowStatusEffect>(DataIndex.ArcaneArrowStatusEffect, out var effect)) return;

                var stack = effect.Stack;

                for (var i = 0; i < stack; i++)
                {
                    executor.Execute(taker);
                }
                
                effect.Dispel();
            });
        }
    }
}
