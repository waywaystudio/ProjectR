namespace Character.Combat.Skill
{
    public class RapidBlow : SkillObject
    {
        public override void InvokeEvent()
        {
            if (DamageModule && TargetModule)
            {
                TargetModule.TakeDamage(DamageModule);
            }
                
        }
    }
}
