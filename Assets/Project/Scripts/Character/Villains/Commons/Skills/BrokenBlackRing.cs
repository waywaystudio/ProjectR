using Common.Projectors;
using Common.Skills;
using UnityEngine;

namespace Character.Villains.Commons.Skills
{
    public class BrokenBlackRing : SkillComponent
    {
        [SerializeField] private DonutProjector projector;
        
        public override void Initialize()
        {
            base.Initialize();
            
            projector.Initialize(this);
            Builder
                .Add(Section.Execute, "HitBrokenBlackRing", HitBrokenBlackRing);
        }
        
        
        private void HitBrokenBlackRing()
        {
            if (!detector.TryGetTakers(out var takers)) return;
            
            takers.ForEach(taker =>
            {
                Taker = taker;
                Invoker.Hit(taker);
            });
        }
    }
}
