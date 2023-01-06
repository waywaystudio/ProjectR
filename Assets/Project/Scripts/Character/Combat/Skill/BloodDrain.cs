namespace Character.Combat.Skill
{
    public class BloodDrain : BaseSkill
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
