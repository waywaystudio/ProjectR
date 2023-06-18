using Common.Skills;
using UnityEngine;

namespace Character.Villains.Moragg.Skills
{
    public class MoraggSpin : SkillComponent, IProjectorSequencer
    {
        public new Vector2 SizeVector => new(range, 60);

        // protected override void PlayAnimation()
        // {
        //     Cb.Animating.PlayLoop(animationKey);
        // }

        public override void Initialize()
        {
            base.Initialize();
            
            SequenceBuilder.Add(SectionType.Execute, "PlayOnceCompleteAnimation",() => Cb.Animating.PlayOnce("attack", 0f, SkillInvoker.Complete))
                           .Add(SectionType.Execute, "CommonExecution", () =>
                           {
                               if (MainTarget is null) return;

                               executor.Execute(MainTarget);
                           });
        }
    }
}
