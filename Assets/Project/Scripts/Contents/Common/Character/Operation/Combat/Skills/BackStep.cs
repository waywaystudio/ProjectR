using Common.Character.Operation.Combat.Entity;
using UnityEngine;

namespace Common.Character.Operation.Combat.Skills
{
    public class BackStep : BaseSkill
    {
        /// <summary>
        /// How far from target behind.
        /// 4.2f almost same as CommonAttack.
        /// </summary>
        [SerializeField] private float backMagnitude = 4.2f;
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
        
        public override void StartSkill()
        {
            if (!TryGetComponent(out TargetEntity targetEntity)) return;
            
            var enemyTransform = targetEntity.CombatTaker.Object.transform;
            var enemyBehindPosition = enemyTransform.position + enemyTransform.forward * -backMagnitude;

            Cb.Teleport(enemyBehindPosition);
            base.StartSkill();
        }
        
        protected override void Reset()
        {
            var skillData = MainGame.MainData.GetSkillData(GetComponent<BaseSkill>().ActionName);
            combatValue = skillData.BaseValue;
        }
    }
}
