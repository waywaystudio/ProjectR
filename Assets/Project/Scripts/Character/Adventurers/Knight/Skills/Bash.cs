using Common.Skills;
// SkillMechanicEntity;

namespace Character.Adventurers.Knight.Skills
{
    public class Bash : SkillComponent
    {
        public override void MainAttack()
        {
            if (!TryGetTakersInSphere(this, out var takerList)) return;
        
            takerList.ForEach(Completion);
        }
        
        protected override void Initialize()
        {
            OnCompleted.Register("EndCallback", End);
        }
    }
}
