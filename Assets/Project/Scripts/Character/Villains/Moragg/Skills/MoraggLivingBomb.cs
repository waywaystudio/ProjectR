using Common.Skills;

namespace Character.Villains.Moragg.Skills
{
    public class MoraggLivingBomb : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();
            
            SequenceBuilder.Add(SectionType.Complete,"MoraggLivingBomb", SkillInvoker.Execute)
                           .Add(SectionType.Execute, "CommonExecution", () => executor.Execute(MainTarget));
        }
    }
}
