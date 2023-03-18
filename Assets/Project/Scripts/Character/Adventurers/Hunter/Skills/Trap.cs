using Common.Skills;

namespace Character.Adventurers.Hunter.Skills
{
    public class Trap : SkillSequence
    {
        // TODO. Require TrapType Projectile

        protected override void Initialize()
        {
            OnActivated.Register("Jump", Jump);
            OnActivated.Register("StartCooling", StartCooling);

            OnCompleted.Register("EndCallback", End);
        }


        private void Jump()
        {
            var direction = Cb.transform.forward * -1f;
            
            Cb.Pathfinding.Jump(direction, 10f, 2, 0.5f);
        }
    }
}
