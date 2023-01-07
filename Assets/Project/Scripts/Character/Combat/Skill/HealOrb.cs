namespace Character.Combat.Skill
{
    public class HealOrb : BaseSkill
    {
        protected override void StartSkill()
        {
            base.StartSkill();

            if (ProjectileEntity && TargetEntity)
                ProjectileEntity.Fire(TargetEntity.Target);
        }
    }
}
