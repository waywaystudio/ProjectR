namespace Character.Combat.Skill
{
    public class Corruption : SkillObject
    {
        protected override void StartSkill()
        {
            base.StartSkill();

            if (StatusEffectModule && TargetModule)
            {
                StatusEffectModule.Effecting(TargetModule.Target);
            }
               
        }
    }
}
