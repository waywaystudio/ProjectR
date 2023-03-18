using Common.Skills;

namespace Character.Adventurers.Knight.Skills
{
    public class ThunderClap : SkillComponent
    {
        public override void MainAttack()
        {
            if (!TryGetTakersInSphere(this, out var takerList)) return;
        
            takerList.ForEach(Completion);
        }


        protected override void Initialize()
        {
            OnActivated.Register("Jump", Jump);
            OnCompleted.Register("EndCallback", End);
        }

        private void Jump()
        {
            var direction = Cb.transform.forward;
            
            Cb.Pathfinding.Jump(direction, 11f, 2.4f, 0.77f);
        }
    }
}
