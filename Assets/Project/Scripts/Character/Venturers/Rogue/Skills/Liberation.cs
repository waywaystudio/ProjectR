using Common;
using Common.Skills;

namespace Character.Venturers.Rogue.Skills
{
    public class Liberation : SkillComponent
    {
        public override ICombatTaker MainTarget => Cb.Searching.GetSelf();

        public override void Execution()
        {
            if (MainTarget is null) return;
            
            ExecutionTable.Execute(MainTarget);
        }

        protected override void Initialize()
        {
            OnCompleted.Register("EndCallback", End);
        }
    }
}
