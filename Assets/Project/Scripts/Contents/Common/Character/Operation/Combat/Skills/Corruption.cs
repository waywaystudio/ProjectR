namespace Common.Character.Operation.Combat.Skills
{
    public class Corruption : BaseSkill
    {
        public override void StartSkill()
        {
            base.StartSkill();
            
            if (StatusEffectEntity && TargetEntity)
                TargetEntity.CombatTakerList.ForEach(target =>
                {
                    target.TakeStatusEffect(StatusEffectEntity);
                });
        }
    }
}
