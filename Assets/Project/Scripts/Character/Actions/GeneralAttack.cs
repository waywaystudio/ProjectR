namespace Character.Actions
{
    public class GeneralAttack : SkillComponent
    {
        protected virtual void OnAttack() { }
        protected override void PlayAnimation()
        {
            CharacterSystem.Animating.PlayOnce(animationKey, progressTime, OnCompleted.Invoke);
        }

        private void RegisterHitEvent()
        {
            CharacterSystem.Animating.OnHit.Register("SkillHit", OnHit.Invoke);
        }

        protected virtual void OnEnable()
        {
            OnActivated.Register("PlayAnimation", PlayAnimation);
            OnActivated.Register("RegisterHitEvent", RegisterHitEvent);
            
            OnHit.Register("GeneralAttack", OnAttack);
            OnCanceled.Register("EndCallback", OnEnded.Invoke);
            OnCompleted.Register("EndCallback", OnEnded.Invoke);

            OnEnded.Register("ReleaseHit", () => CharacterSystem.Animating.OnHit.Unregister("SkillHit"));
            OnEnded.Register("Idle", CharacterSystem.Animating.Idle);
        }
    }
}
