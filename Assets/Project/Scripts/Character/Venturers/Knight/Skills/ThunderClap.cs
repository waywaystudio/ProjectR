using Common.Skills;

namespace Character.Venturers.Knight.Skills
{
    public class ThunderClap : SkillComponent
    {
        public override void Execution()
        {
            if (!TryGetTakersInSphere(this, out var takerList)) return;
        
            takerList.ForEach(Execute);
        }

        protected override void Initialize()
        {
            OnActivated.Register("Jump", Jump);
            OnCompleted.Register("EndCallback", End);
        }

        private void Jump()
        {
            var direction = Cb.transform.forward;
            
            Cb.Pathfinding.Jump(direction, 11f);
        }
    }
}
