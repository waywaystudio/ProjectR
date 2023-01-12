namespace Character.Combat.Skill
{
    public class CommonAttack : SkillObject
    {
        public override void InvokeEvent()
        {
            if (TargetModule && DamageModule)
            {
                TargetModule.TakeDamage(DamageModule);
            }
            
        }
    }
}