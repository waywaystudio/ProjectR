using Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.Character.Operation.Combating
{
    // Range 값과 Character의 무빙 스타일 값을 넣으면 적절한 포지션을 계산해준다.
    // Character의 무빙 스타일 값은 어떤 방식으로 받을 것인가?
    // 1. CharacterBehaviour
    // 2. 어떤 인터페이스 내지는 클래스.
    public class CombatPosition : MonoBehaviour
    {
        [SerializeField] private CharacterBehaviour cb;
        
        private float safeTolerance;
        private float safePoint;
        private Vector3 direction;
        private Vector3 targetPosition;
        private Vector3 characterPosition;

        public CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();

        /// <summary>
        /// Get Suitable Position for use Skill
        /// </summary>
        /// <returns>return true if require to move, return false if already suitable position </returns>
        public bool TryGetCombatPosition(ICombatTaker taker, float range, out Vector3 combatPosition)
        {
            var target = taker?.Taker;

            if (target.IsNullOrEmpty())
            {
                Debug.LogWarning("Target is Null");
                combatPosition = Vector3.negativeInfinity;
                return false;
            }
            
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
            //-----------------------------------Player--------------------------------Target
            // case In Safe Range
            else
            {
                combatPosition = characterPosition;
                return false;
            }

            combatPosition = characterPosition + direction * magnitude;
            return true;
        }
        

        private void Awake()
        {
            safeTolerance = 1f;   // Cb.SafeTolerance
            safePoint = 1f;       // Cb.SafePoint
            
            // safeTolerance = Random.Range(0.3f, 0.7f);   // Cb.SafeTolerance
            // safePoint = Random.Range(0.3f, 0.7f);       // Cb.SafePoint
        }
    }
}

/*
 * 최소 사거리에서 멀어질 경우 참조.
 */
// public bool TryGetCombatPosition(ICombatTaker taker, float range, out Vector3 combatPosition)
//         {
//             var target = taker?.Taker;
//
//             if (target.IsNullOrEmpty())
//             {
//                 Debug.Log("Target is Null");
//                 combatPosition = Vector3.negativeInfinity;
//                 return false;
//             }
//             
//             targetPosition = target.transform.position;
//             characterPosition = transform.position;
//             
//             var currentDistance = Vector3.Distance(targetPosition, characterPosition);
//             float magnitude;
//             
//             //--outOfRange - MaxRange -<            SafeRange          >- CloseRange - Target
//             //--------------------------safeRange.x----------safeRange.y---------------Target
//             //------------------------------------safeTolerance------------------------Target
//             //---------Player----------------------------------------------------------Target
//             // case Out of Range
//             if (currentDistance > range)
//             {
//                 direction = (targetPosition - characterPosition).normalized;
//                 magnitude = Mathf.Abs(currentDistance - range) + range * (safeTolerance * safePoint);
//             }
//             
//             //--outOfRange - MaxRange -<            SafeRange          >- CloseRange - Target
//             //--------------------------safeRange.x----------safeRange.y---------------Target
//             //------------------------------------safeTolerance------------------------Target
//             //--------------------------------------------------------------Player-----Target
//             // case Too Close Range
//             else if (currentDistance <= range * (1.0f - safeTolerance))
//             {
//                 direction = (targetPosition - characterPosition).normalized * -1.0f;
//                 magnitude = Mathf.Abs(currentDistance - range) - range * (safeTolerance * safePoint);
//             }
//             
//             //--outOfRange - MaxRange -<            SafeRange          >- CloseRange - Target
//             //--------------------------safeRange.x----------safeRange.y---------------Target
//             //------------------------------------safeTolerance------------------------Target
//             //-----------------------------------Player--------------------------------Target
//             // case In Safe Range
//             else
//             {
//                 combatPosition = characterPosition;
//                 return false;
//             }
//
//             combatPosition = characterPosition + direction * magnitude;
//             return true;
//         }