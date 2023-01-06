namespace Character.Combat.Skill
{
    public class Fury : BaseSkill
    {
        protected override void StartSkill()
        {
            base.StartSkill();

            if (TargetEntity && StatusEffectEntity)
            {
                TargetEntity.Target.TakeStatusEffect(StatusEffectEntity);
            }
        }
    }
}
