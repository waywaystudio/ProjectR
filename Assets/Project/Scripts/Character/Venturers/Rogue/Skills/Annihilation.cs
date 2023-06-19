using Common.Skills;

namespace Character.Venturers.Rogue.Skills
{
    public class Annihilation : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();
            
            SequenceBuilder.Add(SectionType.Execute, "CommonExecution", () => detector.GetTakers()?.ForEach(executor.Execute));
        }
    }
}
