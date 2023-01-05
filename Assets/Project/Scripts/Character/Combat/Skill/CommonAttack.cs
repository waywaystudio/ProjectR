namespace Character.Combat.Skill
{
    public class CommonAttack : BaseSkill
    {
        public override void InvokeEvent()
        {
            if (DamageEntity && TargetEntity)
                TargetEntity.CombatTakerList.ForEach(target =>
                {
                    target.TakeDamage(DamageEntity);
                });
        }
    }
}