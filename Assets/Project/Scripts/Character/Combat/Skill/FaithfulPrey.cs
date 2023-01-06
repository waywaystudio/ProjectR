namespace Character.Combat.Skill
{
    public class FaithfulPrey : BaseSkill
    {
        protected override void CompleteSkill()
        {
            if (HealEntity && TargetEntity)
                TargetEntity.Target.TakeHeal(HealEntity);
            // {
            //     TargetEntity.TakerList.ForEach(target => target.TakeHeal(HealEntity));
            // }

            base.CompleteSkill();
        }
    }
}
