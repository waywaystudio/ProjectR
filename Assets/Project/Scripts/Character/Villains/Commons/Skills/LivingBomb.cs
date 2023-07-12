using Common.Skills;

namespace Character.Villains.Commons.Skills
{
    public class LivingBomb : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();
            
            Builder
                .Add(Section.Complete,"MoraggLivingBomb", Invoker.Execute)
                .Add(Section.Execute, "CommonExecution", () => Invoker.Hit(MainTarget));
        }
    }
}
