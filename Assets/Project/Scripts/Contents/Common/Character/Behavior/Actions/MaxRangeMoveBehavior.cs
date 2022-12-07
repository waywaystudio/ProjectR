using BehaviorDesigner.Runtime.Tasks;
using Core;
using UnityEngine;

namespace Common.Character.Behavior.Actions
{
    [TaskCategory("Character")]
    public class MaxRangeMoveBehavior : Action
    {
        private OLD_CharacterBehaviour pb;
        private float range;
        private float safeTolerance;
        private float safePoint;
        private GameObject target;

        private Vector3 direction;
        private Vector3 targetPosition;
        private Vector3 characterPosition;

        public override void OnAwake()
        {
            pb = GetComponent<OLD_CharacterBehaviour>();
            // range = pb.Searching.AttackRange;
            safeTolerance = Random.Range(0.3f, 0.7f);
            safePoint = Random.Range(0.3f, 0.7f);
        }

        public override TaskStatus OnUpdate()
        {
            // target = pb.Searching.FocusTarget;

            if (target.IsNullOrEmpty()) return TaskStatus.Failure;
            
            targetPosition = target.transform.position;
            characterPosition = transform.position;

            var currentDistance = Vector3.Distance(targetPosition, characterPosition);
            float magnitude;
            

            //--outOfRange - MaxRange -<            SafeRange          >- CloseRange - Target
            //--------------------------safeRange.x----------safeRange.y---------------Target
            //------------------------------------safeTolerance------------------------Target
            //---------Player----------------------------------------------------------Target
            // case Out of Range
            if (currentDistance > range)
            {
                direction = (targetPosition - characterPosition).normalized;
                magnitude = Mathf.Abs(currentDistance - range) + range * (safeTolerance * safePoint);
            }
            
            //--outOfRange - MaxRange -<            SafeRange          >- CloseRange - Target
            //--------------------------safeRange.x----------safeRange.y---------------Target
            //------------------------------------safeTolerance------------------------Target
            //--------------------------------------------------------------Player-----Target
            // case Too Close Range
            else if (currentDistance <= range * (1.0f - safeTolerance))
            {
                direction = (targetPosition - characterPosition).normalized * -1.0f;
                magnitude = Mathf.Abs(currentDistance - range) - range * (safeTolerance * safePoint);
            }
            
            //--outOfRange - MaxRange -<            SafeRange          >- CloseRange - Target
            //--------------------------safeRange.x----------safeRange.y---------------Target
            //------------------------------------safeTolerance------------------------Target
            //-----------------------------------Player--------------------------------Target
            // case In Safe Range
            else // if (range * (1.0f - safeTolerance) < currentDistance && currentDistance <= range)
            {
                return pb.IsDestinationReached 
                    ? TaskStatus.Success 
                    : TaskStatus.Failure;
                // return TaskStatus.Success;
            }

            var destination = characterPosition + direction * magnitude;

            pb.Run(destination);

            return TaskStatus.Failure;
        }
    }
}
