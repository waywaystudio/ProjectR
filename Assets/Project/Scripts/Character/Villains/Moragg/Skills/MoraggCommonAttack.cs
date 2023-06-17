using Common.Skills;

namespace Character.Villains.Moragg.Skills
{
    public class MoraggCommonAttack : SkillComponent
    {
        public override void Execution() => ExecuteAction.Invoke();

        protected override void AddSkillSequencer()
        {
            AddAnimationEvent();
            
            ExecuteAction.Add("CommonExecution", () =>
            {
                if (MainTarget is null) return;

                executor.Execute(MainTarget);
            });
        }
    }
}
