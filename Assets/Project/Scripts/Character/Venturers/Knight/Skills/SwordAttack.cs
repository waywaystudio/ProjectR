using Common.Skills;

namespace Character.Venturers.Knight.Skills
{
    public class SwordAttack : SkillComponent
    {
        public override void Execution()
        {
            if (!TryGetTakersInSphere(this, out var takerList)) return;
        
            takerList.ForEach(Execute);
        }
        
        
        protected override void Initialize()
        {
            OnCompleted.Register("EndCallback", End);
        }
    }
}

