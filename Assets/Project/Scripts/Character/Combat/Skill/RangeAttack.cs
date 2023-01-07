namespace Character.Combat.Skill
{
    public class RangeAttack : BaseSkill
    {
        protected override void CompleteSkill()
        {
            base.CompleteSkill();

            if (TargetEntity && ProjectileEntity)
                ProjectileEntity.Fire(TargetEntity.Target);
        }
    }
}
