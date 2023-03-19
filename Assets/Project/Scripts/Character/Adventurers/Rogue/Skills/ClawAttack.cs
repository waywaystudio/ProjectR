using Common.Skills;

namespace Character.Adventurers.Rogue.Skills
{
    public class ClawAttack : SkillComponent
    {
        public override void Execution()
        {
            if (!TryGetTakersInSphere(this, out var takerList)) return;
        
            takerList.ForEach(Executor.Execute);
        }

        protected override void Initialize()
        {
            OnCompleted.Register("EndCallback", End);
        }
    }
}
