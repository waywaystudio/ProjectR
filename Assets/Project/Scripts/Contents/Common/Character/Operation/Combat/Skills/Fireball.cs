namespace Common.Character.Operation.Combat.Skills
{
    public class Fireball : BaseSkill
    {
        public override void CompleteSkill()
        {
            if (TargetEntity && ProjectileEntity)
            {
                TargetEntity.CombatTakerList.ForEach(target =>
                {
                    ProjectileEntity.Fire(target);
                });
            }

            base.CompleteSkill();
        }
    }
}
