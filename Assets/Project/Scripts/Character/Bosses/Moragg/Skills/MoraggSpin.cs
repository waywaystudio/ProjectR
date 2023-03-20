using Common.Projectors;
using Common.Skills;
using UnityEngine;

namespace Character.Bosses.Moragg.Skills
{
    public class MoraggSpin : SkillComponent
    {
        [SerializeField] private SphereProjector projector;


        public override void Execution()
        {
            if (!TryGetTakersInSphere(this, out var takerList)) return;
            
            takerList.ForEach(ExecutionTable.Execute);
        }

        protected override void PlayAnimation()
        {
            Cb.Animating.PlayLoop(animationKey);
        }

        protected override void Initialize()
        {
            // TODO. Projector.Initialize 에서 CastingTime을 받아오면 안되고, Active 에서 받아야 한다.
            projector.Initialize(3f, range, this);

            OnCompleted.Register("MoraggSpinAttack", Execution);
            OnCompleted.Register("PlayEndCastingAnimation", PlayEndCastingAnimation);
        }

        private void PlayEndCastingAnimation()
        {
            Cb.Animating.PlayOnce("attack", 0f, End);
        }
    }
}
