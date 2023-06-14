using Common.Skills;
// SkillMechanicEntity;

namespace Character.Venturers.Knight.Skills
{
    public class Bash : SkillComponent
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
