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

        private CharacterBehaviour cb;

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
            
            // TODO. enemyBehindPosition 에 PathfindingUtility.BFS 알고리즘을 이용하여 안전한 장소로 순간이동 시켜보자.

            cb.Teleport(enemyBehindPosition);
        }
        
        protected override void OnAssigned()
        {
            cb = GetComponentInParent<CharacterBehaviour>();
            OnHit.Register(InstanceID, OnBackStepHit);
            OnActivated.Register(InstanceID, OnBackStepActive);
        }
    }
}
