namespace Character.Actions.Soldier
{
    public class Trap : SkillComponent
    {
        // TODO. Require TrapType Projectile
        
        public override void Release() { }
        
        protected override void PlayAnimation()
        {
            CharacterSystem.Animating.PlayOnce(animationKey, progressTime, OnCompleted.Invoke);
        }
        
        private void Jump()
        {
            var providerTransform = Provider.Object.transform;
            var jumpDirection     = providerTransform.forward * -1f;
            
            // TODO. Forward로 바꾼후, 플레이어는 마우스 포지션을 받아, 즉시 회전 후 Forward로 점프하는 형태.
            var dest = providerTransform.position + jumpDirection * 10f;
            
            CharacterSystem.Pathfinding.Jump(dest, 10f);
        }
        
        protected void OnEnable()
        {
            OnActivated.Register("Jump", Jump);
            OnActivated.Register("StartCooling", StartCooling);

            OnCompleted.Register("EndCallback", OnEnded.Invoke);
        }
    }
}
