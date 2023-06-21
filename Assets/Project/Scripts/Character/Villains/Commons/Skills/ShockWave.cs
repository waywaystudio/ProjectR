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
            
            var animationTable = Cb.AnimationTable;
            
            SequenceBuilder.Add(SectionType.Execute, "CommonExecution", () => detector.GetTakers()?.ForEach(executor.Execute))
                           // .Add(SectionType.Execute, "PlayOnceCompleteAnimation",() => Cb.Animating.PlayOnce("attack", 0f, SkillInvoker.Complete))
                           .Add(SectionType.Execute, "PlayAnimatorOnceCompleteAnimation", () => animationTable.Play("Attack", 1f, SkillInvoker.Complete));
        }
    }
}
