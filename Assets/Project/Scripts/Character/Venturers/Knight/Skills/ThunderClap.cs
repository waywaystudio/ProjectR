using Common.Skills;

namespace Character.Venturers.Knight.Skills
{
    public class ThunderClap : SkillComponent
    {
        public override void Execution() => ExecuteAction.Invoke();

        protected override void AddSkillSequencer()
        {
            AddAnimationEvent();

            SequenceBuilder.Add(SectionType.Active,"Jump", Jump);

            ExecuteAction.Add("CommonExecution", () =>
            {
                if (!TryGetTakersInSphere(this, out var takerList)) return;
        
                takerList.ForEach(executor.Execute);
            });
        }

        private void Jump()
        {
            var direction = Cb.transform.forward;
            
            Cb.Pathfinding.Jump(direction, 11f);
        }
    }
}
