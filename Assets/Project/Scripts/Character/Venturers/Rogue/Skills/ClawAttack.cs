using Common.Skills;

namespace Character.Venturers.Rogue.Skills
{
    public class ClawAttack : SkillComponent
    {
        public override void Execution()
        {
            if (!TryGetTakersInSphere(this, out var takerList)) return;
        
            takerList.ForEach(Execute);
        }

        protected override void Initialize()
        {
            // OnCompleted.Register("EndCallback", End);
        }
    }
}
