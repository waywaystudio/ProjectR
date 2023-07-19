using Character.Venturers.Ranger.StatusEffects;
using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Ranger.Skills
{
    public class Reload : SkillComponent
    {
        private readonly Collider[] buffers = new Collider[32];
        
        public override void Initialize()
        {
            base.Initialize();

            Builder.Add(Section.Execute, "DamageByArrowsCount", CollectArrows);
        }


        private void CollectArrows()
        {
            if (!detector.TryGetTakersInCircle(transform.position, 100f, buffers, out var takers)) return;

            takers.ForEach(taker =>
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
