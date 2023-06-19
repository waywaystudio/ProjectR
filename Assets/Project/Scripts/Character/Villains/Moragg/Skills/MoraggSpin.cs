using Common.Skills;
using UnityEngine;

namespace Character.Villains.Moragg.Skills
{
    public class MoraggSpin : SkillComponent, IProjectorSequencer
    {
        public Vector2 SizeVector => new(Range, 60);

        public override void Initialize()
        {
            base.Initialize();
            
            SequenceBuilder.Add(SectionType.Execute, "PlayOnceCompleteAnimation",() => Cb.Animating.PlayOnce("attack", 0f, SkillInvoker.Complete))
                           .Add(SectionType.Execute, "CommonExecution", () => executor.Execute(MainTarget));
        }
    }
}
