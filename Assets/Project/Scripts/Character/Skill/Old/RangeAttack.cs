namespace Character.Combat.Skill
{
    public class RangeAttack : SkillObject
    {
        private void OnRangeAttackCompleted()
        {
            if (TargetModule && ProjectileModule)
            {
                TargetModule.TakeProjectile(ProjectileModule);
            }
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            OnCompleted.Register(InstanceID, OnRangeAttackCompleted);
        }
    }
}
