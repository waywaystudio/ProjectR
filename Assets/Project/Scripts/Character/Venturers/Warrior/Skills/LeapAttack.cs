using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Warrior.Skills
{
    public class LeapAttack : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();

            SequenceBuilder.AddActiveParam("Jump", Jump)
                           .Add(SectionType.Execute, "CommonExecution", () => detector.GetTakers()?.ForEach(executor.Execute));
        }
        

        private void Jump(Vector3 targetPosition)
        {
            var venturer = GetComponentInParent<VenturerBehaviour>();
            var destination = venturer.IsPlayer
                ? targetPosition
                : detector.GetMainTarget() is not null
                    ? detector.GetMainTarget().Position
                    : Vector3.zero;

            var playerPosition = Cb.transform.position;
            var direction = (destination - playerPosition).normalized;
            var actualDistance = Vector3.Distance(destination, playerPosition);

            if (actualDistance > Range)
            {
                actualDistance = Range;
            }
            
            Cb.Pathfinding.Jump(direction, actualDistance);
        }
    }
}
