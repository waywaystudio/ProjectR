using Common.Completion;
using Common.Skills;
using UnityEngine;

namespace Monsters.Moragg.Skills
{
    public class MoraggLivingBomb : SkillComponent
    {
        [SerializeField] private StatusEffectCompletion livingBomb;
        
        
        protected override void PlayAnimation()
        {
            Cb.Animating.PlayOnce(animationKey, progressTime, OnCompleted.Invoke);
        }

        private void OnAttack()
        {
            if (MainTarget is null) return;
            
            livingBomb.DeBuff(MainTarget);
        }
        
        private void RegisterHitEvent()
        {
            Cb.Animating.OnHit.Register("SkillHit", OnHit.Invoke);
        }

        private void OnEnable()
        {
            OnActivated.Register("PlayAnimation", PlayAnimation);
            OnActivated.Register("RegisterHitEvent", RegisterHitEvent);
            
            OnHit.Register("MoraggLivingBomb", OnAttack);
            
            OnCanceled.Register("EndCallback", OnEnded.Invoke);
            
            OnCompleted.Register("StartCooling", StartCooling);
            OnCompleted.Register("EndCallback", OnEnded.Invoke);
            
            OnEnded.Register("ReleaseHit", () => Cb.Animating.OnHit.Unregister("SkillHit"));
            OnEnded.Register("Idle", Cb.Animating.Idle);
        }
    }
}
