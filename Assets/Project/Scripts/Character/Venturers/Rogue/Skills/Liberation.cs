using Common;
using Common.Skills;

namespace Character.Venturers.Rogue.Skills
{
    public class Liberation : SkillComponent
    {
        public override ICombatTaker MainTarget => Cb.Searching.GetSelf();

        public override void Initialize()
        {
            base.Initialize();
            
            SequenceBuilder.Add(SectionType.Execute, "CommonExecution", () =>
            {
                if (MainTarget is null) return;

                executor.Execute(MainTarget);
            });
        }
    }
}
