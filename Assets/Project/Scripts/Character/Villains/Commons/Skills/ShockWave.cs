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
            
            SequenceBuilder.Add(SectionType.Execute, "PlayOnceCompleteAnimation",() => Cb.Animating.PlayOnce("attack", 0f, SkillInvoker.Complete))
                           .Add(SectionType.Execute, "CommonExecution", () => detector.GetTakers()?.ForEach(executor.Execute));
        }
    }
}
