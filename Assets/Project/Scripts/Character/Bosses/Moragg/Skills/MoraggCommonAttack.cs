using Common.Completion;
using Common.Skills;
using UnityEngine;

namespace Character.Bosses.Moragg.Skills
{
    public class MoraggCommonAttack : SkillComponent
    {
        [SerializeField] protected PowerCompletion power;
        [SerializeField] protected StatusEffectCompletion armorCrash;
        
        
        protected override void PlayAnimation()
        {
            Cb.Animating.PlayOnce(animationKey, progressTime, OnCompleted.Invoke);
        }
        
        private void OnAttack()
        {
            power.Damage(MainTarget);
            armorCrash.DeBuff(MainTarget);
        }
        
        private void RegisterHitEvent()
        {
            Cb.Animating.OnHit.Register("SkillHit", OnHit.Invoke);
        }
        
        private void OnEnable()
        {
            power.Initialize(Cb, ActionCode);
            armorCrash.Initialize(Cb);

            OnActivated.Register("PlayAnimation", PlayAnimation);
            OnActivated.Register("RegisterHitEvent", RegisterHitEvent);
            
            OnHit.Register("MoraggCommonAttack", OnAttack);
            OnCanceled.Register("EndCallback", OnEnded.Invoke);
            OnCompleted.Register("EndCallback", OnEnded.Invoke);

            OnEnded.Register("ReleaseHit", () => Cb.Animating.OnHit.Unregister("SkillHit"));
            OnEnded.Register("Idle", Cb.Animating.Idle);
        }
    }
}
