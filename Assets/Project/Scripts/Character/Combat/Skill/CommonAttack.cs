namespace Character.Combat.Skill
{
    public class CommonAttack : SkillObject
    {
        private void OnCommonAttackHit()
        {
            if (TargetModule && DamageModule)
            {
                TargetModule.TakeDamage(DamageModule);
            }
        }
        
        protected override void OnAssigned()
        {
            OnHit.Register(InstanceID, OnCommonAttackHit);
        }
    }
}