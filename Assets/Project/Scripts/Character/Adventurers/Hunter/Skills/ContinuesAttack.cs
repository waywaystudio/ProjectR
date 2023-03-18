using Common.Skills;
using UnityEngine;

namespace Character.Adventurers.Hunter.Skills
{
    public class ContinuesAttack : SkillSequence
    {
        [SerializeField] private DirectHitEvent directHitEvent;

        public override void OnAttack()
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
            
            takerList.ForEach(directHitEvent.Completion);
        }
        
        
        protected override void Initialize()
        {
            directHitEvent.Initialize();
            
            OnCompleted.Register("EndCallback", End);
            OnEnded.Register("StartCooling", StartCooling);
        }

        protected override void PlayAnimation()
        {
            Cb.Animating.PlayLoop(animationKey);
        }


#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();

            TryGetComponent(out directHitEvent);
        }
#endif
    }
}
