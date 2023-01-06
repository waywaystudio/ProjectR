namespace Character.Combat.Skill
{
    public class Corruption : BaseSkill
    {
        protected override void StartSkill()
        {
            base.StartSkill();
            
            if (StatusEffectEntity && TargetEntity)
                TargetEntity.Target.TakeStatusEffect(StatusEffectEntity);
                // TargetEntity.TakerList.ForEach(target =>
                // {
                //     target.TakeStatusEffect(StatusEffectEntity);
                // });
        }
    }
}
