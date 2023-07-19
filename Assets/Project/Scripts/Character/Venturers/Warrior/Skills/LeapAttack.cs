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
            if (!detector.TryGetTakers(out var takers)) return;
            
            takers.ForEach(taker =>
            {
                Taker = taker;
                Invoker.Hit(taker);
            });
        }

        private void Jump(Vector3 targetPosition)
        {
            var venturer = GetComponentInParent<VenturerBehaviour>();
            var playerPosition = Cb.transform.position;
            var destination = venturer.IsPlayer
                ? TargetUtility.GetValidPosition(Cb.transform.position, PivotRange, targetPosition)
                : detector.GetMainTarget() is not null
                    ? detector.GetMainTarget().Position
                    : Vector3.zero;

            var direction = (destination - playerPosition).normalized;
            var actualDistance = Vector3.Distance(destination, playerPosition);

            Cb.Pathfinding.Jump(direction, actualDistance);
        }
    }
}
