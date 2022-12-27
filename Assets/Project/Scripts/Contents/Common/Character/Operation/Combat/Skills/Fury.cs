namespace Common.Character.Operation.Combat.Skills
{
    public class Fury : BaseSkill
    {
        public override void StartSkill()
        {
            base.StartSkill();

            if (TargetEntity && StatusEffectEntity)
            {
                TargetEntity.CombatTaker.TakeStatusEffect(StatusEffectEntity);
            }
        }
    }
}
