using Common.Skills;

namespace Character.Venturers.Ranger.Skills
{
    public class ShotAttack : SkillComponent
    {
        public override void Execution() => ExecuteAction.Invoke();
        
        protected override void AddSkillSequencer()
        {
            AddAnimationEvent();
            
            ExecuteAction.Add("ShotAttackExecution", () => executor.Execute(null)); 
        }
    }
}
