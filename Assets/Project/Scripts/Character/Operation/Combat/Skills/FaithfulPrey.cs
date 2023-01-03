namespace Common.Character.Operation.Combat.Skills
{
    public class FaithfulPrey : BaseSkill
    {
        public override void CompleteSkill()
        {
            if (HealEntity && TargetEntity)
            {
                TargetEntity.CombatTakerList.ForEach(target => target.TakeHeal(HealEntity));
            }

            base.CompleteSkill();
        }
    }
}
