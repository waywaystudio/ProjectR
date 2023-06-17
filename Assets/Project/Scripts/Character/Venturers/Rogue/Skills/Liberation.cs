using Common;
using Common.Skills;

namespace Character.Venturers.Rogue.Skills
{
    public class Liberation : SkillComponent
    {
        public override ICombatTaker MainTarget => Cb.Searching.GetSelf();

        public override void Execution() => ExecuteAction.Invoke();

        protected override void AddSkillSequencer()
        {
            ExecuteAction.Add("CommonExecution", () =>
            {
                if (MainTarget is null) return;

                executor.Execute(MainTarget);
            });
        }
    }
}
