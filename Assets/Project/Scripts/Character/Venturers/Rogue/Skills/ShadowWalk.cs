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

            SequenceBuilder.AddActiveParam("TeleportPathfinding", Teleport);
            //.Add(SectionType.Complete, "CreatePhantom", CreatePhantom);
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
            CreatePhantom();
        }
        
        private void CreatePhantom()
        {
            executor.Execute(ExecuteGroup.Group2, Cb.transform.position);
        }

        private bool HasTarget()
        {
            var takers = detector.GetTakers();

            return !takers.IsNullOrEmpty() 
                   && takers[0].DynamicStatEntry.Alive.Value;
        }


#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();

            phantomMaster = GetComponentInParent<PhantomMaster>();
        }
#endif
    }
}
