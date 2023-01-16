namespace Character.Combat.Skill
{
    public class Fireball : SkillObject
    {
        private void OnFireballCompleted()
        {
            if (TargetModule && ProjectileModule)
            {
                TargetModule.TakeProjectile(ProjectileModule);
            }
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            OnCompleted.Register(InstanceID, OnFireballCompleted);
        }
    }
}
