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
            // TODO. Projector.Initialize 에서 CastingTime을 받아오면 안되고, Active 에서 받아야 한다.
            projector.Initialize(3f, range, this);

            OnCompleted.Register("MoraggSpinAttack", MainAttack);
            OnCompleted.Register("PlayEndCastingAnimation", PlayEndCastingAnimation);
        }

        private void PlayEndCastingAnimation()
        {
            Cb.Animating.PlayOnce("attack", 0f, End);
        }
    }
}
