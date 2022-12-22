using Common.Character.Operation.Combat.Entity;
using UnityEngine;

namespace Common.Character.Operation.Combat.Skills
{
    public class ChainHealing : BaseSkill
    {
        [SerializeField] private float combatValue;
        
        public override CombatValueEntity CombatValue
        {
            get
            {
                var healValue = Cb.CombatValue;
                healValue.Power = Cb.CombatValue.Power * combatValue;
                healValue.Hit = 1.0f;

                return healValue;
            }
        }
        
        public override void CompleteSkill()
        {
            var hasTargetList = TryGetComponent(out TargetEntity targetEntity);
            var hasProjectile = TryGetComponent(out ProjectileEntity projectileEntity);

            if (hasTargetList && hasProjectile)
            {
                targetEntity.CombatTakerList.ForEach(target =>
                {
                    projectileEntity.Initialize(target);
                    projectileEntity.OnArrived.Register(InstanceID, () => target.TakeHeal(this));
                });
            }

            base.CompleteSkill();
        }
        
        protected override void Reset()
        {
            var skillData = MainGame.MainData.GetSkillData(GetComponent<BaseSkill>().ActionName);
            combatValue = skillData.BaseValue;
        }
    }
}
