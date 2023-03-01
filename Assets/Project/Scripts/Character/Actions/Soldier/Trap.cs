namespace Character.Actions.Soldier
{
    public class Trap : SkillComponent
    {
        // TODO. Require TrapType Projectile
        
        
        protected override void PlayAnimation()
        {
            CharacterSystem.Animating.PlayOnce(animationKey, progressTime, OnCompleted.Invoke);
        }
        
        // TODO. 백점프를 하다보니, Flip이 이상해지는 구간이 생긴다.
        private void Jump()
        {
            var direction = Provider.Object.transform.forward * -1f;
            
            CharacterSystem.Pathfinding.Jump(direction, 10f);
        }
        
        protected void OnEnable()
        {
            OnActivated.Register("Jump", Jump);
            OnActivated.Register("StartCooling", StartCooling);

            OnCompleted.Register("EndCallback", OnEnded.Invoke);
        }
    }
}
