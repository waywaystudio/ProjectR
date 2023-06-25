using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Rogue.Skills
{
    public class ShadowWalk : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();

            // AddCondition, is there any Rogue Dummy
            SequenceBuilder.AddActiveParam("TeleportPathfinding", Teleport);
            //.Add(SectionType.Execute, "CommonExecution",() => detector.GetTakers()?.ForEach(executor.Execute));
        }


        private void Teleport(Vector3 targetPosition)
        {
            var venturer = GetComponentInParent<VenturerBehaviour>();
            var destination = venturer.IsPlayer
                ? targetPosition
                : detector.GetMainTarget() is not null
                    ? detector.GetMainTarget().Position
                    : Vector3.zero;

            var playerPosition = Cb.transform.position;
            var direction = (targetPosition - playerPosition).normalized;
            var actualDistance = Vector3.Distance(destination, playerPosition);

            if (actualDistance > Range)
            {
                actualDistance = Range;
            }

            Cb.Pathfinding.Teleport(direction, actualDistance);
        }

        private bool HasTarget()
        {
            var takers = detector.GetTakers();

            return !takers.IsNullOrEmpty() 
                   && takers[0].DynamicStatEntry.Alive.Value;
        }
    }
}
