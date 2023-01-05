namespace Character.Combat.Skill
{
    public class FaithfulPrey : BaseSkill
    {
        protected override void CompleteSkill()
        {
            if (HealEntity && TargetEntity)
            {
                TargetEntity.CombatTakerList.ForEach(target => target.TakeHeal(HealEntity));
            }

            base.CompleteSkill();
        }
    }
}
