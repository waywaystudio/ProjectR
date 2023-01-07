using UnityEngine;

namespace Character.Combat.Skill
{
    public class BackStep : BaseSkill
    {
        /// <summary>
        /// How far from target behind.
        /// 4.2f almost same as CommonAttack.
        /// </summary>
        [SerializeField] private float backMagnitude = 4.2f;

        public override void InvokeEvent()
        {
            if (DamageEntity && TargetEntity)
            {
                TargetEntity.Target.TakeDamage(DamageEntity);
            }
        }
        
        protected override void StartSkill()
        {
            if (!TargetEntity) return;
            
            var enemyTransform = TargetEntity.Target.Object.transform;
            var enemyBehindPosition = enemyTransform.position + enemyTransform.forward * -backMagnitude;

            Cb.Teleport(enemyBehindPosition);
            base.StartSkill();
        }
    }
}
