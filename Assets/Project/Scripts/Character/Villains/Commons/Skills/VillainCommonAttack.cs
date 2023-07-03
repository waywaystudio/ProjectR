using Common.Skills;

namespace Character.Villains.Commons.Skills
{
    public class VillainCommonAttack : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();
            
            Builder.Add(SectionType.Execute, "CommonExecution", () => executor.ToTaker(MainTarget));
        }
    }
}
