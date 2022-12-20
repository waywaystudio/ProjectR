using Common.Character.Operation.Combating.Entity;

namespace Common.Character.Operation.Combating.Skills
{
    public class BloodDrain : BaseSkill
    {
        public override void StartSkill()
        {
            base.StartSkill();
            
            var hasStatusEffect = TryGetComponent(out StatusEffectEntity statusEffectEntity);
            var hasTargetList = TryGetComponent(out TargetEntity targetEntity);

            if (hasStatusEffect && hasTargetList)
            {
                targetEntity.CombatTaker.TakeBuff(statusEffectEntity.Name, statusEffectEntity);
            }
        }
    }
}
