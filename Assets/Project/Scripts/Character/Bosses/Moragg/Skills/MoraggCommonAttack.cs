using Common.Skills;

namespace Character.Bosses.Moragg.Skills
{
    public class MoraggCommonAttack : SkillComponent
    {
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
