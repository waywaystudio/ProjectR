using Common.Skills;

namespace Character.Villains.Moragg.Skills
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
            Sequencer.CompleteAction.Add("MoraggLivingBomb", Execution);
            // OnCompleted.Register("EndCallback", End);
        }
    }
}
