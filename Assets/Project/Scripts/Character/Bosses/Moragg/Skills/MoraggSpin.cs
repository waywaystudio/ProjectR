using Common.Completion;
using Common.Projectors;
using Common.Skills;
using UnityEngine;

namespace Character.Bosses.Moragg.Skills
{
    public class MoraggSpin : SkillSequence
    {
        [SerializeField] private float knockBackDistance = 5f;
        [SerializeField] private DamageCompletion damage;
        [SerializeField] private SphereProjector projector;
        
        public override void OnAttack()
        {
            if (!TryGetTakersInSphere(this, out var takerList)) return;
            
            takerList.ForEach(taker =>
            {
                damage.Completion(taker);
                taker.KnockBack(Cb.transform.position, knockBackDistance);
            });
        }
        
        
        protected override void PlayAnimation()
        {
            Cb.Animating.PlayLoop(animationKey);
        }
        
        protected override void Initialize()
        {
            damage.Initialize(Cb);
            projector.Initialize(progressTime, range, this);

            OnActivated.Register("StartProgress", () => StartProgression(Complete));

            OnCompleted.Register("MoraggSpinAttack", OnAttack);
            OnCompleted.Register("PlayEndCastingAnimation", PlayEndCastingAnimation);
            OnCompleted.Register("StartCooling", StartCooling);
            
            OnEnded.Register("StopProgress", StopProgression);
        }

        private void PlayEndCastingAnimation()
        {
            Cb.Animating.PlayOnce("attack", 0f, End);
        }
    }
}
