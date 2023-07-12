using Common.Projectors;
using Common.Skills;
using UnityEngine;

namespace Character.Villains.Commons.Skills
{
    public class ShockWave : SkillComponent
    {
        [SerializeField] private ArcProjector projector;
        
        public override void Initialize()
        {
            base.Initialize();
            
            projector.Initialize(this);
            Builder
                .Add(Section.Execute, "CommonExecution", HitShockWave);
        }


        private void HitShockWave()
        {
            detector.GetTakers()?.ForEach(Invoker.Hit);
        }
    }
}
