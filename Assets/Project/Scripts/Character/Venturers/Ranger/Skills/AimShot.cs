using Common.Skills;

namespace Character.Venturers.Ranger.Skills
{
    public class AimShot : SkillComponent
    {
        public override void Execution() => ExecuteAction.Invoke();
        

        protected override void AddSkillSequencer()
        {
            ExecuteAction.Add("PlayEndChargingAnimation", PlayEndChargingAnimation);
            ExecuteAction.Add("AimShotExecute", () => executor.Execute(null));
        }
        
        protected override void PlayAnimation()
        {
            Cb.Animating.PlayOnce(animationKey);
        }

        private void PlayEndChargingAnimation()
        {
            Cb.Animating.PlayOnce("heavyAttack", 0f, SequenceInvoker.Complete);
        }
    }
}
