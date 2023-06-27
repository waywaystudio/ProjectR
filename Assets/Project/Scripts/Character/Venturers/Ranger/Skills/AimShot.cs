using Common.Skills;

namespace Character.Venturers.Ranger.Skills
{
    public class AimShot : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();

            SequenceBuilder.Add(SectionType.Execute, "ShotAttackExecution", () => executor.Execute(null))
                           .Add(SectionType.Execute, "PlayCastCompleteAnimation", PlayCastCompleteAnimation);

        }
        
        
        private void PlayCastCompleteAnimation()
        {
            Cb.Animating.PlayOnce("AttackSlashHoldFire", 1f + Haste, SkillInvoker.Complete);
        }
    }
}
