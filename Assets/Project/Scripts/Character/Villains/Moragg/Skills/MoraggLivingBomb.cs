using Common.Skills;

namespace Character.Villains.Moragg.Skills
{
    public class MoraggLivingBomb : SkillComponent
    {
        public override void Execution() => ExecuteAction.Invoke();

        protected override void AddSkillSequencer()
        {
            ExecuteAction.Add("CommonExecution", () =>
            {
                if (MainTarget is null) return;

                executor.Execute(MainTarget);
            });

            SequenceBuilder.AddComplete("MoraggLivingBomb", Execution);
        }
    }
}
