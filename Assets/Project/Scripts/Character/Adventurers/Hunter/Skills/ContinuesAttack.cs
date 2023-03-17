using Common.Completion;
using Common.Skills;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character.Adventurers.Hunter.Skills
{
    public class ContinuesAttack : SkillComponent
    {
        [FormerlySerializedAs("power")] [SerializeField] private DamageCompletion damage;
        
        
        protected override void PlayAnimation()
        {
            Cb.Animating.PlayLoop(animationKey);
        }
        
        protected new void RegisterHitEvent()
        {
            Cb.Animating.OnHit.Unregister("HoldingHit");
            Cb.Animating.OnHit.Register("HoldingHit", OnHit.Invoke);
        }
        
        protected override void Initialize()
        {
            damage.Initialize(Cb, ActionCode);
            
            OnActivated.Register("RegisterHitEvent", RegisterHitEvent);
            OnHit.Register("OnHoldingAttack", OnHoldingAttack);
            OnCompleted.Register("EndCallback", End);

            OnEnded.Register("StartCooling", StartCooling);
            OnEnded.Register("ReleaseHit", () => Cb.Animating.OnHit.Unregister("HoldingHit"));
        }

        protected override void Dispose()
        {
            // TODO. Unregister Sequence Events;
        }

        private void OnHoldingAttack()
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
