using Common.Skills;

namespace Character.Venturers.Knight.Skills
{
    public class ThunderClap : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();

            SequenceBuilder.Add(SectionType.Active, "Jump", Jump)
                           .Add(SectionType.Execute, "CommonExecution", () => detector.GetTakers()?.ForEach(executor.Execute));
        }

        private void Jump()
        {
            var direction = Cb.transform.forward;
            Cb.Pathfinding.Jump(direction, 11f);
        }
    }
}
