using Common.Character.Operation.Combat.Entity;

namespace Common.Character.Operation.Combat.Skills
{
    public class Corruption : BaseSkill
    {
        public override CombatValueEntity CombatValue => Cb.CombatValue;
        
        public override void StartSkill()
        {
            base.StartSkill();
            
            // var hasStatusEffect = TryGetComponent(out StatusEffectEntity statusEffectEntity);
            var hasTargetList = TryGetComponent(out TargetEntity targetEntity);
            
            if (hasTargetList)
                targetEntity.CombatTakerList.ForEach(target =>
                {
                    target.TakeStatusEffect(this);
                });
        }
    }
}
