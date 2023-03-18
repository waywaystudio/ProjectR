using Common.Completion;
using Common.Projectors;
using Common.Skills;
using UnityEngine;

namespace Character.Bosses.Moragg.Skills
{
    public class MoraggSpin : SkillComponent
    {
        [SerializeField] private float knockBackDistance = 5f;
        [SerializeField] private SphereProjector projector;
        
        
        public override void MainAttack()
        {
            if (!TryGetTakersInSphere(this, out var takerList)) return;
            
            takerList.ForEach(taker =>
            {
                Completion(taker);
                taker.KnockBack(Cb.transform.position, knockBackDistance);
            });
        }
        

        protected override void PlayAnimation()
        {
            Cb.Animating.PlayLoop(animationKey);
        }
        
        protected override void Initialize()
        {
            projector.Initialize(progressTime, range, this);

            OnActivated.Register("StartProgress", () => StartProgression(Complete));

            OnCompleted.Register("MoraggSpinAttack", MainAttack);
            OnCompleted.Register("PlayEndCastingAnimation", PlayEndCastingAnimation);

            OnEnded.Register("StopProgress", StopProgression);
        }

        private void PlayEndCastingAnimation()
        {
            Cb.Animating.PlayOnce("attack", 0f, End);
        }
    }
}
