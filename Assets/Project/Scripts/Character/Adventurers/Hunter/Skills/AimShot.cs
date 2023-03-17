using Common.Completion;
using Common.Skills;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character.Adventurers.Hunter.Skills
{
    public class AimShot : SkillComponent
    {
        [FormerlySerializedAs("power")] [SerializeField] private DamageCompletion damage;
        
        protected override void PlayAnimation()
        {
            Cb.Animating.PlayOnce(animationKey);
        }
        
        protected override void Initialize()
        {
            damage.Initialize(Cb, ActionCode);
            
            OnCompleted.Register("StartCooling", StartCooling);
            OnCompleted.Register("OnAimShotAttack", OnAimShotAttack);
            OnCompleted.Register("PlayEndChargingAnimation", PlayEndChargingAnimation);
            OnCompleted.Register("StopProcess", StopProgression);
        }

        protected override void Dispose()
        {
            // TODO. Unregister Sequence Events;
        }
        
        private void OnAimShotAttack()
        {
            // TODO. 현재 Test상 HitScan 방식이어서 이렇고, Projectile로 바뀌면 교체해야 함.
            var providerTransform = Cb.transform;
            
            if (!Cb.Colliding.TryGetTakersByRaycast(
                    providerTransform.position,
                    providerTransform.forward,
                    range,
                    16,
                    targetLayer,
                    out var takerList)) return;

            takerList.ForEach(damage.Damage);
        }
        
        private void PlayEndChargingAnimation()
        {
            Cb.Animating.PlayOnce("heavyAttack", 0f, End);
        }


#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var skillData = Database.SkillSheetData(actionCode);

            if (!TryGetComponent(out damage))
            {
                damage = gameObject.AddComponent<DamageCompletion>();
            }

            damage.SetDamage(skillData.CompletionValueList[0]);
        }
#endif
    }
}
