using Common.Character.Operation.Combat.Entity;
using UnityEngine;

namespace Common.Character.Operation.Combat.Skills
{
    public class Charge : BaseSkill
    {
        [SerializeField] private float dashSpeed = 30.0f;
        [SerializeField] private float offsetDistance = 3.5f;
        [SerializeField] private float combatValue;

        public override CombatValueEntity CombatValue
        {
            get
            {
                var damageValue = Cb.CombatValue;
                damageValue.Power = Cb.CombatValue.Power * combatValue;

                return damageValue;
            }
        }
        
        public override void InvokeEvent()
        {
            var hasTargetList = TryGetComponent(out TargetEntity targetEntity);
            
            if (hasTargetList)
                targetEntity.CombatTakerList.ForEach(target => target.TakeDamage(this));
        }

        // 위 처럼 구현하고 매 프레임에서 Target != null을 체크하면 안정성이 조금 올라간다.
        public override void ActiveSkill()
        {
            if (!TryGetComponent(out TargetEntity targetEntity)) return;

            var takerTransform = targetEntity.CombatTaker.Object.transform;
            var offset = Cb.Direction.Invoke() * (offsetDistance * -1f);
            var targetFrontPosition = takerTransform.position + offset;
            
            Cb.MoveSpeedTable.Register("Charge", () => dashSpeed, true);
            Cb.Run(targetFrontPosition, base.ActiveSkill);
        }

        public override void CompleteSkill()
        {
            base.CompleteSkill();
            Cb.MoveSpeedTable.Unregister("Charge");
        }
        
        protected override void Reset()
        {
            var skillData = MainGame.MainData.GetSkillData(GetComponent<BaseSkill>().ActionName);
            combatValue = skillData.BaseValue;
        }
    }
}
