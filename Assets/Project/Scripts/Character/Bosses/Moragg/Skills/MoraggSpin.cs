using Common;
using Common.Completion;
using Common.Projectors;
using Common.Skills;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character.Bosses.Moragg.Skills
{
    public class MoraggSpin : SkillComponent
    {
        [SerializeField] private float knockBackDistance = 5f;
        [FormerlySerializedAs("power")] [SerializeField] private DamageCompletion damage;
        [SerializeField] private SphereProjector projector;
        
        protected override void PlayAnimation()
        {
            Cb.Animating.PlayLoop(animationKey);
        }
        
        protected override void Initialize()
        {
            var self = GetComponentInParent<ICombatTaker>();
            
            damage.Initialize(Cb, ActionCode);
            
            projector.Initialize(progressTime, range);
            projector.SetTaker(self);
            projector.AssignTo(this);

            OnActivated.Register("PlayCastingAnimation", PlayAnimation);
            OnActivated.Register("StartProgress", () => StartProgression(Complete));

            OnCompleted.Register("MoraggSpinAttack", OnMoraggSpinAttack);
            OnCompleted.Register("PlayEndCastingAnimation", PlayEndCastingAnimation);
            OnCompleted.Register("StartCooling", StartCooling);
            
            OnEnded.Register("StopProgress", StopProgression);
        }

        protected override void Dispose()
        {
            // TODO. Unregister Sequence Events;
        }
        
        
        private void OnMoraggSpinAttack()
        {
            if (!TryGetTakersInSphere(this, out var takerList)) return;
            
            takerList.ForEach(taker =>
            {
                damage.Damage(taker);
                taker.KnockBack(Cb.transform.position, knockBackDistance);
            });
        }
        
        private void PlayEndCastingAnimation()
        {
            Cb.Animating.PlayOnce("attack", 0f, End);
        }
    }
}
