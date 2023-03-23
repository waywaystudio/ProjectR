using Common.Skills;

namespace Character.Adventurers.Knight.Skills
{
    public class SwordAttack : SkillComponent
    {
        public override void Execution()
        {
            if (!TryGetTakersInSphere(this, out var takerList)) return;
        
            takerList.ForEach(ExecutionTable.Execute);
        }
        
        
        protected override void Initialize()
        {
            OnCompleted.Register("EndCallback", End);
        }
    }
}

