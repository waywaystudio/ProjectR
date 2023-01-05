namespace Character.Combat.Skill
{
    public class ChainHealing : BaseSkill
    {
        protected override void CompleteSkill()
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
