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

        private void OnBackStepHit()
        {
            if (DamageModule && TargetModule)
            {
                TargetModule.TakeDamage(DamageModule);
            }
        }

        private void OnBackStepActive()
        {
            if (!TargetModule) return;
            
            var enemyTransform = TargetModule.Target.Object.transform;
            var enemyBehindPosition = enemyTransform.position + enemyTransform.forward * -backMagnitude;

            Debug.Log($"Teleport! to:{enemyBehindPosition}");
            Cb.Teleport(enemyBehindPosition);
        }

        protected override void Awake()
        {
            base.Awake();
            
            OnHit.Register(InstanceID, OnBackStepHit);
            OnActivated.Register(InstanceID, OnBackStepActive);
        }
    }
}
