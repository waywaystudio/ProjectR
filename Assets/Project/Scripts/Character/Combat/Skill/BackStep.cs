using UnityEngine;

namespace Character.Combat.Skill
{
    public class BackStep : SkillObject
    {
        /// <summary>
        /// How far from target behind.
        /// 4.2f almost same as CommonAttack.
        /// </summary>
        [SerializeField] private float backMagnitude = 4.2f;

        public override void InvokeEvent()
        {
            if (DamageModule && TargetModule)
            {
                TargetModule.TakeDamage(DamageModule);
            }
        }
        
        protected override void StartSkill()
        {
            if (!TargetModule) return;
            
            var enemyTransform = TargetModule.Target.Object.transform;
            var enemyBehindPosition = enemyTransform.position + enemyTransform.forward * -backMagnitude;

            Cb.Teleport(enemyBehindPosition);
            base.StartSkill();
        }
    }
}
