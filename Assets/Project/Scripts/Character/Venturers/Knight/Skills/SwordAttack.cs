using Common.Skills;

namespace Character.Venturers.Knight.Skills
{
    public class SwordAttack : SkillComponent
    {
        public override void Execution() => ExecuteAction.Invoke();
        
        protected override void AddSkillSequencer()
        {
            AddAnimationEvent();
            
            ExecuteAction.Add("CommonExecution", () =>
            {
                if (!TryGetTakersInSphere(this, out var takerList)) return;
        
                takerList.ForEach(executor.Execute);
            });
        }
    }
}

