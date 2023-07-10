using Common;
using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Warrior.Skills
{
    public class LeapAttack : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();

            Builder
                .AddApplying("Jump", Jump)
                .Add(Section.Execute, "ExecuteLeapAttack", ExecuteLeapAttack);
        }


        private void ExecuteLeapAttack()
        {
            detector.GetTakers()?.ForEach(Invoker.Hit);
        }

        private void Jump(Vector3 targetPosition)
        {
            var venturer = GetComponentInParent<VenturerBehaviour>();
            var playerPosition = Cb.transform.position;
            var destination = venturer.IsPlayer
                ? TargetUtility.GetValidPosition(Cb.transform.position, Range, targetPosition)
                : detector.GetMainTarget() is not null
                    ? detector.GetMainTarget().Position
                    : Vector3.zero;

            // var playerPosition = Cb.transform.position;
            var direction = (destination - playerPosition).normalized;
            var actualDistance = Vector3.Distance(destination, playerPosition);

            Cb.Pathfinding.Jump(direction, actualDistance);
        }
    }
}
