using Common.Projectors;
using Common.Skills;
using UnityEngine;
using Projector = Common.Projectors.Projector;

namespace Character.Villains.Commons.Skills
{
    public class ShockWave : SkillComponent
    {
        [SerializeField] private Projector projector;
        
        public override void Initialize()
        {
            base.Initialize();
            
            projector.Initialize(this);
            Builder
                .Add(Section.Execute, "HitShockWave", HitShockWave);
        }


        private void HitShockWave()
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
