using Common.Skills;

namespace Character.Adventurers.Hunter.Skills
{
    public class Trap : SkillComponent
    {
        // TODO. Require TrapType Projectile
        
        
        protected override void PlayAnimation()
        {
            Cb.Animating.PlayOnce(animationKey, progressTime, OnCompleted.Invoke);
        }

        private void Jump()
        {
            var direction = Cb.transform.forward * -1f;
            
            Cb.Pathfinding.Jump(direction, 10f, 2, 0.5f);
        }
        
        protected void OnEnable()
        {
            OnActivated.Register("Jump", Jump);
            OnActivated.Register("StartCooling", StartCooling);

            OnCompleted.Register("EndCallback", OnEnded.Invoke);
        }
    }
}
