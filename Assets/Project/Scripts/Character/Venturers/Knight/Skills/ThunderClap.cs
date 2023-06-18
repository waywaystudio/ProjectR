using Common.Skills;

namespace Character.Venturers.Knight.Skills
{
    public class ThunderClap : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();
            
            // AddAnimationEvent();

            SequenceBuilder.Add(SectionType.Active, "Jump", Jump)
                           .Add(SectionType.Execute, "CommonExecution", () =>
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
