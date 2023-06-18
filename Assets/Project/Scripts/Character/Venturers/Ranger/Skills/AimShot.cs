using Common.Skills;

namespace Character.Venturers.Ranger.Skills
{
    public class AimShot : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();
            
            SequenceBuilder.Add(SectionType.Execute, "PlayEndChargingAnimation", PlayEndChargingAnimation)
                           .Add(SectionType.Execute, "AimShotExecute", () => executor.Execute(null));
        }
        
        // protected override void PlayAnimation()
        // {
        //     Cb.Animating.PlayOnce(animationKey);
        // }

        private void PlayEndChargingAnimation()
        {
            Cb.Animating.PlayOnce("heavyAttack", 0f, SkillInvoker.Complete);
        }
    }
}
