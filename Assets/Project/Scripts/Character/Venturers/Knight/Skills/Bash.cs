using Common.Skills;

namespace Character.Venturers.Knight.Skills
{
    public class Bash : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();

            SequenceBuilder.Add(SectionType.Execute, "CommonExecution",
                                () => detector.GetTakers()?.ForEach(executor.Execute));
        }
    }
}
