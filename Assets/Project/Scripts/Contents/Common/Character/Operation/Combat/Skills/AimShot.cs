namespace Common.Character.Operation.Combat.Skills
{
    public class AimShot : BaseSkill
    {
        public override void CompleteSkill()
        {
            base.CompleteSkill();

            if (TargetEntity && ProjectileEntity)
            {
                TargetEntity.CombatTakerList.ForEach(target =>
                {
                    ProjectileEntity.Fire(target);
                });
            }
        }
    }
}
