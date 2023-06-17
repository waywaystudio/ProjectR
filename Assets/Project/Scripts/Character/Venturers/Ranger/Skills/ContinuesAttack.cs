using Common.Skills;

namespace Character.Venturers.Ranger.Skills
{
    public class ContinuesAttack : SkillComponent
    {
        public override void Execution() => ExecuteAction.Invoke();

        protected override void AddSkillSequencer()
        {
            AddAnimationEvent();
            
            ExecuteAction.Add("ShotAttackExecution", () => executor.Execute(null));
        }

        protected override void PlayAnimation()
        {
            Cb.Animating.PlayLoop(animationKey);
        }
    }
}
