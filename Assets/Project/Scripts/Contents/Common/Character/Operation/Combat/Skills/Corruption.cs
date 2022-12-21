using Common.Character.Operation.Combat.Entity;

namespace Common.Character.Operation.Combat.Skills
{
    public class Corruption : BaseSkill
    {
        public override void StartSkill()
        {
            base.StartSkill();
            
            var hasStatusEffect = TryGetComponent(out StatusEffectEntity statusEffectEntity);
            var hasTargetList = TryGetComponent(out TargetEntity targetEntity);
            
            if (hasStatusEffect && hasTargetList)
                targetEntity.CombatTakerList.ForEach(target =>
                {
                    target.TakeDeBuff(statusEffectEntity);
                });
        }
    }
}
