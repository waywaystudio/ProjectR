using Common.Skills;

namespace Character.Villains.Commons.Skills
{
    public class LivingBomb : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();
            
            Builder.Add(SectionType.Complete,"MoraggLivingBomb", Invoker.Execute)
                           .Add(SectionType.Execute, "CommonExecution", () => executor.ToTaker(MainTarget));
        }
    }
}
