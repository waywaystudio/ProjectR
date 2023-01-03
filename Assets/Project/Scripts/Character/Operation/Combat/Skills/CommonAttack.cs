namespace Common.Character.Operation.Combat.Skills
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