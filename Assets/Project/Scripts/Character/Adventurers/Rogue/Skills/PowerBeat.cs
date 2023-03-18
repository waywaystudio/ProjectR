using Common.Skills;

namespace Character.Adventurers.Rogue.Skills
{
    public class PowerBeat : SkillComponent
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
