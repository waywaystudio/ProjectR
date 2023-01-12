namespace Character.Combat.Skill
{
    public class Fury : SkillObject
    {
        protected override void StartSkill()
        {
            base.StartSkill();

            if (TargetModule && StatusEffectModule)
            {
                TargetModule.TakeStatusEffect(StatusEffectModule);
            }
                
        }
    }
}
