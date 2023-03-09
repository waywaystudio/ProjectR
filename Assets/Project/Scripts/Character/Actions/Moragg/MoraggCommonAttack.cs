using UnityEngine;

namespace Character.Actions.Moragg
{
    public class MoraggCommonAttack : SkillComponent
    {
        [SerializeField] protected ValueCompletion power;
        
        
        protected override void PlayAnimation()
        {
            CharacterSystem.Animating.PlayOnce(animationKey, progressTime, OnCompleted.Invoke);
        }
        
        private void OnAttack()
        {
            power.Damage(MainTarget);
        }
        
        private void RegisterHitEvent()
        {
            CharacterSystem.Animating.OnHit.Register("SkillHit", OnHit.Invoke);
        }
        
        private void OnEnable()
        {
            power.Initialize(Provider, ActionCode);

            OnActivated.Register("PlayAnimation", PlayAnimation);
            OnActivated.Register("RegisterHitEvent", RegisterHitEvent);
            
            OnHit.Register("MoraggCommonAttack", OnAttack);
            OnCanceled.Register("EndCallback", OnEnded.Invoke);
            OnCompleted.Register("EndCallback", OnEnded.Invoke);

            OnEnded.Register("ReleaseHit", () => CharacterSystem.Animating.OnHit.Unregister("SkillHit"));
            OnEnded.Register("Idle", CharacterSystem.Animating.Idle);
        }
    }
}
