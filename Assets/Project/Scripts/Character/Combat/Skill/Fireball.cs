namespace Character.Combat.Skill
{
    public class Fireball : BaseSkill
    {
        protected override void CompleteSkill()
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
