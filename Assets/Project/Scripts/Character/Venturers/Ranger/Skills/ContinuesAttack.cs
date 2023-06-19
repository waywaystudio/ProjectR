using Common.Skills;

namespace Character.Venturers.Ranger.Skills
{
    public class ContinuesAttack : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();

            SequenceBuilder.Add(SectionType.Execute, "ShotAttackExecution", () => executor.Execute(null));
        }
    }
}
