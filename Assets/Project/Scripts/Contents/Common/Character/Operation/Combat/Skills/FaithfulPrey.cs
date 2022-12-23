using Common.Character.Operation.Combat.Entity;

namespace Common.Character.Operation.Combat.Skills
{
    public class FaithfulPrey : BaseSkill
    {
        public override void CompleteSkill()
        {
            var hasHeal = TryGetComponent(out HealEntity healEntity);
            var hasTargetList = TryGetComponent(out TargetEntity targetEntity);

            if (hasHeal && hasTargetList)
            {
                targetEntity.CombatTakerList.ForEach(target => target.TakeHeal(healEntity));
            }

            base.CompleteSkill();
        }
    }
}
