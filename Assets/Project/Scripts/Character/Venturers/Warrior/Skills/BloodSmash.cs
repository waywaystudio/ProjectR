using Common.Skills;

namespace Character.Venturers.Warrior.Skills
{
    public class BloodSmash : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();

            SequenceBuilder.Add(SectionType.Execute, "CommonExecution", 
                                () => detector.GetTakers()?.ForEach(executor.Execute));
        }
    }
}
