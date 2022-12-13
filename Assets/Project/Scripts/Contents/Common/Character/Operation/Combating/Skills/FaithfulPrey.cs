using Common.Character.Operation.Combating.Entity;

namespace Common.Character.Operation.Combating.Skills
{
    public class FaithfulPrey : BaseSkill
    {
        public override void CompleteSkill()
        {
            var hasProvider = TryGetComponent(out HealEntity healEntity);
            var hasTargetList = TryGetComponent(out TargetEntity targetEntity);

            if (hasProvider && hasTargetList)
            {
                targetEntity.CombatTakerList.ForEach(target => target.TakeHeal(healEntity));
            }
            
            // Set Projectile Information before projectile Fire
            // base.CompleteSkill == Projectile Fire
            base.CompleteSkill();
        }
    }
}
