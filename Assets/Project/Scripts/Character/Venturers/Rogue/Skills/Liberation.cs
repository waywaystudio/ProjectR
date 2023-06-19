using Common.Skills;

namespace Character.Venturers.Rogue.Skills
{
    public class Liberation : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();
            
            SequenceBuilder.Add(SectionType.Execute, "CommonExecution", () => executor.Execute(MainTarget));
        }
    }
}
