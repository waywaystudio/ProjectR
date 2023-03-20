using Common.Skills;

namespace Character.Bosses.Moragg.Skills
{
    public class MoraggLivingBomb : SkillComponent
    {
        public override void Execution()
        {
            if (MainTarget is null) return;

            ExecutionTable.Execute(MainTarget);
        }

        protected override void Initialize()
        {
            OnCompleted.Register("MoraggLivingBomb", Execution);
            OnCompleted.Register("EndCallback", End);
        }
    }
}
