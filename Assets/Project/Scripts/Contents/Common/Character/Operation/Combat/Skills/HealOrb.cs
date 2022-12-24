namespace Common.Character.Operation.Combat.Skills
{
    public class HealOrb : BaseSkill
    {
        public override void StartSkill()
        {
            base.StartSkill();

            if (ProjectileEntity && TargetEntity)
            {
                TargetEntity.CombatTakerList.ForEach(target =>
                {
                    ProjectileEntity.Fire(target);
                });
            }
        }
    }
}
