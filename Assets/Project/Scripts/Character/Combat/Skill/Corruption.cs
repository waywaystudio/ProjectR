namespace Character.Combat.Skill
{
    public class Corruption : BaseSkill
    {
        protected override void StartSkill()
        {
            base.StartSkill();

            if (StatusEffectEntity && TargetEntity)
            {
                StatusEffectEntity.Effecting(TargetEntity.Target);
            }
               
        }
    }
}
