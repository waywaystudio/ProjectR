using Common.Skills;

namespace Character.Villains.Moragg.Skills
{
    public class MoraggCommonAttack : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();
            
            // AddAnimationEvent();
            
            SequenceBuilder.Add(SectionType.Execute, "CommonExecution", () =>
            {
                if (MainTarget is null) return;

                executor.Execute(MainTarget);
            });
        }
    }
}
