namespace Character.Combat.Skill
{
    public class CommonAttack : SkillObject
    {
        public override void InvokeEvent()
        {
            TargetModule.TakeDamage(DamageModule);
        }
    }
}