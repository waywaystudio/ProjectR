namespace Character.Combat.Skill
{
    public class Fireball : BaseSkill
    {
        protected override void CompleteSkill()
        {
            if (TargetEntity && ProjectileEntity)
                ProjectileEntity.Fire(TargetEntity.Target);

            base.CompleteSkill();
        }
    }
}
