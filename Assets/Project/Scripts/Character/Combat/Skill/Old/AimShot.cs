namespace Character.Combat.Skill
{
    public class AimShot : SkillObject
    {
        private void OnAimShotCompleted()
        {
            if (TargetModule && ProjectileModule)
            {
                TargetModule.TakeProjectile(ProjectileModule);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            
            OnCompleted.Register(InstanceID, OnAimShotCompleted);
        }
    }
}
