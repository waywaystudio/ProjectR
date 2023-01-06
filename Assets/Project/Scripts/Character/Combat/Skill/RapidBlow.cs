namespace Character.Combat.Skill
{
    public class RapidBlow : BaseSkill
    {
        public override void InvokeEvent()
        {
            if (DamageEntity && TargetEntity)
                TargetEntity.Target.TakeDamage(DamageEntity);
                // TargetEntity.TakerList.ForEach(target => target.TakeDamage(DamageEntity));
        }
    }
}
