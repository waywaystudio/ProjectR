using Common.Skills;

namespace Character.Venturers.Ranger.Skills
{
    public class ContinuesAttack : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();
            
            // AddAnimationEvent();

            SequenceBuilder.Add(SectionType.Execute, "ShotAttackExecution", () => executor.Execute(null));
        }

        // protected override void PlayAnimation()
        // {
        //     Cb.Animating.PlayLoop(animationKey);
        // }
    }
}
