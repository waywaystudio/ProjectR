namespace Character.Combat.Skill
{
    public class RapidBlow : SkillObject
    {
        private void OnRapidBlowHit()
        {
            if (DamageModule && TargetModule)
            {
                TargetModule.TakeDamage(DamageModule);
            }
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            OnHit.Register(InstanceID, OnRapidBlowHit);
        }
    }
}
