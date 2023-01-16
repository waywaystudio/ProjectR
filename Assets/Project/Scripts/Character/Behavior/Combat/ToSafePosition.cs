using BehaviorDesigner.Runtime.Tasks;

namespace Character.Behavior.Combat
{
    [TaskCategory("Character/Combat")]
    public class ToSafePosition : Action
    {
        private CharacterBehaviour cb;
        
        public override void OnAwake()
        {
            cb = gameObject.GetComponentInParent<CharacterBehaviour>();
        }

        public override TaskStatus OnUpdate()
        {
            var selfPosition = cb.transform.position;
            var safePosition = PathfindingUtility.GetNearestSafePosition(selfPosition, 3f);

            cb.Run(safePosition);

            return TaskStatus.Success;
        }
    }
}
