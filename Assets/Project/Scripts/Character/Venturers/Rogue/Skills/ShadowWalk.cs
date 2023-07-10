using Common;
using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Rogue.Skills
{
    public class ShadowWalk : SkillComponent
    {
        [SerializeField] private PhantomMaster phantomMaster;
        
        public override void Initialize()
        {
            base.Initialize();

            Builder
                .AddApplying("TeleportPathfinding", Teleport);
        }


        private void Teleport(Vector3 targetPosition)
        {
            var playerPosition = Cb.transform.position;
            var direction = (targetPosition - playerPosition).normalized;
            var venturer = GetComponentInParent<VenturerBehaviour>();
            var destination = venturer.IsPlayer
                ? targetPosition
                : detector.GetMainTarget() is not null
                    ? detector.GetMainTarget().Position + direction
                    : Vector3.zero;

            
            var actualDistance = Vector3.Distance(destination, playerPosition);

            if (actualDistance > Range)
            {
                actualDistance = Range;
            }

            Cb.Pathfinding.Teleport(direction, actualDistance);
            CreatePhantom();
        }
        
        private void CreatePhantom()
        {
            Invoker.SubFire(Cb.transform.position);
        }
    }
}
