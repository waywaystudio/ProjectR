namespace Character.Skill
{
    public class GeneralAttack : SkillComponent
    {
        public override void Release() { }

        protected virtual void OnAttack() { }
        protected override void PlayAnimation()
        {
            model.PlayOnce(animationKey, progressTime, OnCompleted.Invoke);
        }

        private void RegisterHitEvent()
        {
            model.OnHit.Register("SkillHit", OnHit.Invoke);
        }

        protected virtual void OnEnable()
        {
            OnActivated.Register("PlayAnimation", PlayAnimation);
            OnActivated.Register("RegisterHitEvent", RegisterHitEvent);
            
            OnHit.Register("GeneralAttack", OnAttack);
            OnCanceled.Register("EndCallback", OnEnded.Invoke);
            OnCompleted.Register("EndCallback", OnEnded.Invoke);

            OnEnded.Register("ReleaseHit", () => model.OnHit.Unregister("SkillHit"));
            OnEnded.Register("Idle", model.Idle);
        }
    }
}
