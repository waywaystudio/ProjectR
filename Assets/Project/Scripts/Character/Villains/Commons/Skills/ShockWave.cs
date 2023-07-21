using Common.Projectors;
using Common.Skills;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character.Villains.Commons.Skills
{
    public class ShockWave : SkillComponent
    {
        [FormerlySerializedAs("projector")] [SerializeField] private Projection projection;
        
        public override void Initialize()
        {
            base.Initialize();
            
            projection.Initialize(this);
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
