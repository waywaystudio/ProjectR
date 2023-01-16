namespace Character.Combat.Skill
{
    public class HealOrb : SkillObject
    {
        private void OnHealOrbActivated()
        {
            if (ProjectileModule && TargetModule)
            {
                TargetModule.TakeProjectile(ProjectileModule);
            }
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            OnActivated.Register(InstanceID, OnHealOrbActivated);
        }
    }
}
