using Common.Skills;

namespace Character.Villains.Commons.Skills
{
    public class LivingBomb : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();
            
            SequenceBuilder.Add(SectionType.Complete,"MoraggLivingBomb", SkillInvoker.Execute)
                           .Add(SectionType.Execute, "CommonExecution", () => executor.Execute(MainTarget));
        }
    }
}
