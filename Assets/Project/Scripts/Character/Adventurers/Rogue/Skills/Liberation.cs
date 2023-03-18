using Common;
using Common.Skills;

namespace Adventurers.Rogue.Skills
{
    public class Liberation : SkillSequence
    {
        public override ICombatTaker MainTarget => Cb.Searching.GetSelf();

        protected override void Initialize()
        {
            OnActivated.Register("StartCooling", StartCooling);
            OnCompleted.Register("EndCallback", End);
        }
    }
}
