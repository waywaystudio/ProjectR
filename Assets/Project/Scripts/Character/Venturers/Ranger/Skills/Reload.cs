using Character.Venturers.Ranger.StatusEffects;
using Common.Characters;
using Common.Skills;

namespace Character.Venturers.Ranger.Skills
{
    public class Reload : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();

            Builder.Add(Section.Execute, "DamageByArrowsCount", CollectArrows);
        }


        private void CollectArrows()
        {
            var takers = detector.GetTakersInCircleRange(100f, 360f);
            
            takers?.ForEach(taker =>
            {
                if (!taker.StatusEffectTable
                          .TryGetEffect<ArcaneArrowStatusEffect>(DataIndex.ArcaneArrowStatusEffect, out var effect)) return;

                Taker = taker;

                for (var i = 0; i < effect.Stack; i++)
                {
                    Invoker.Hit(taker);
                }
                
                effect.Dispel();
            });
        }
    }
}
