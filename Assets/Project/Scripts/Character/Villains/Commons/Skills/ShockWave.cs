using Common.Skills;
using UnityEngine;

namespace Character.Villains.Commons.Skills
{
    public class ShockWave : SkillComponent, IProjectorSequencer
    {
        public Vector2 SizeVector => new(Range, Angle);

        public override void Initialize()
        {
            base.Initialize();
            
            Builder.Add(SectionType.Execute, "PlayOnceCompleteAnimation",() => Cb.Animating.PlayOnce("attack", 1f + Haste, Invoker.Complete))
                           .Add(SectionType.Execute, "CommonExecution", () => detector.GetTakers()?.ForEach(executor.ToTaker));
        }
    }
}
