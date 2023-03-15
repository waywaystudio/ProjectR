using Common;
using Common.Completion;
using Common.Projectors;
using Common.Skills;
using UnityEngine;

namespace Character.Bosses.Moragg.Skills
{
    public class MoraggSpin : SkillComponent
    {
        [SerializeField] private float knockBackDistance = 5f;
        [SerializeField] private PowerCompletion power;
        [SerializeField] private SphereProjector projector;
        
        protected override void PlayAnimation()
        {
            CharacterSystem.Animating.PlayLoop(animationKey);
        }
        
        private void OnMoraggSpinAttack()
        {
            if (!TryGetTakersInSphere(this, out var takerList)) return;
            
            takerList.ForEach(taker =>
            {
                power.Damage(taker);
                taker.KnockBack(Provider.Object.transform.position, knockBackDistance);
            });
        }
        
        private void PlayEndCastingAnimation()
        {
            CharacterSystem.Animating.PlayOnce("attack", 0f, OnEnded.Invoke);
        }
        
        private void OnEnable()
        {
            var self = GetComponentInParent<ICombatTaker>();
            
            power.Initialize(Provider, ActionCode);
            projector.Initialize(progressTime, range);
            projector.SetTaker(self);
            projector.AssignTo(this);

            OnActivated.Register("PlayCastingAnimation", PlayAnimation);
            OnActivated.Register("StartProgress", () => StartProgression(OnCompleted.Invoke));

            OnCanceled.Register("EndCallback", OnEnded.Invoke);
            
            OnCompleted.Register("MoraggSpinAttack", OnMoraggSpinAttack);
            OnCompleted.Register("PlayEndCastingAnimation", PlayEndCastingAnimation);
            OnCompleted.Register("StartCooling", StartCooling);
            
            OnEnded.Register("StopProgress", StopProgression);
            OnEnded.Register("Idle", CharacterSystem.Animating.Idle);
        }
    }
}
