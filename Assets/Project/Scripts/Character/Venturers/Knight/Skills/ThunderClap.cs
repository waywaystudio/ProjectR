using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Knight.Skills
{
    public class ThunderClap : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();

            SequenceBuilder.AddActiveParam("Jump", Jump)
                           .Add(SectionType.Execute, "CommonExecution", () => detector.GetTakers()?.ForEach(executor.Execute));
        }
        

        private void Jump(Vector3 direction)
        {
            Cb.Pathfinding.Jump(direction, 11f);
        }
    }
}
