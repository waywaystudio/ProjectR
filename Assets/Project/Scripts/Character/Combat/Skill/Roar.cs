namespace Character.Combat.Skill
{
    public class Roar : SkillObject
    {
        private void OnRoarActivated()
        {
            if (TargetModule)
            {
                if (DamageModule) TargetModule.TakeDamage(DamageModule);
                if (StatusEffectModule) TargetModule.TakeStatusEffect(StatusEffectModule);
            }
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            OnActivated.Register(InstanceID, OnRoarActivated);
        }
    }
}
