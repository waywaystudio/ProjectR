using Common.Skills;

namespace Character.Villains.Commons.Skills
{
    public class VillainCommonAttack : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();

            SequenceBuilder.Add(SectionType.Execute, "CommonExecution", () => executor.Execute(MainTarget));
        }
    }
}
