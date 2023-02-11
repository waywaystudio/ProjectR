namespace Character.Combat.Skill
{
    public class ChainHealing : SkillObject
    {
        private void OnChainHealingCompleted()
        {
            if (TargetModule && ProjectileModule)
            {
                TargetModule.TakeProjectile(ProjectileModule);
            }
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            OnCompleted.Register(InstanceID, OnChainHealingCompleted);
        }
    }
}
