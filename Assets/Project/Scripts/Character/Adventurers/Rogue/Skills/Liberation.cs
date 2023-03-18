using Common;
using Common.Skills;

namespace Adventurers.Rogue.Skills
{
    public class Liberation : SkillComponent
    {
        public override ICombatTaker MainTarget => Cb.Searching.GetSelf();

        public override void MainAttack() { }

        protected override void Initialize()
        {
            OnCompleted.Register("EndCallback", End);
        }
    }
}
