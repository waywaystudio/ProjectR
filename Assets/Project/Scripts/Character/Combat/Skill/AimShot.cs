namespace Character.Combat.Skill
{
    public class AimShot : BaseSkill
    {
        protected override void CompleteSkill()
        {
            base.CompleteSkill();

            if (TargetEntity && ProjectileEntity)
            {
                ProjectileEntity.Fire(TargetEntity.Target);

                // TargetEntity.TakerList.ForEach(target =>
                // {
                //     ProjectileEntity.Fire(target);
                // });
            }
        }
    }
}
