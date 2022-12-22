using Common.Character.Operation.Combat.Entity;

namespace Common.Character.Operation.Combat.Skills
{
    public class BloodDrain : BaseSkill
    {
        public override CombatValueEntity CombatValue => Cb.CombatValue;
        
        public override void StartSkill()
        {
            base.StartSkill();

            var hasTargetList = TryGetComponent(out TargetEntity targetEntity);

            if (hasTargetList)
            {
                targetEntity.CombatTaker.TakeStatusEffect(this);
            }
        }
    }
}
