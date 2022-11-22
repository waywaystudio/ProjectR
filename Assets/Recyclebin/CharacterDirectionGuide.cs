// TODO. Flip이 발생할 때, 하단에 전후 화살표를 Tweening 하는 메소드 구현이 필요했는데
// AIPath가 자동으로 로테이션을 트위닝 해주고 있다.

// using UnityEngine;
//
// namespace Common.Character
// {
//     public class CharacterDirectionGuide : MonoBehaviour
//     {
//         public void MatchForward(Vector3 direction)
//         {
//             if (direction.Equals(Vector3.zero)) return;
//             
//             var forward = transform.position + direction;
//             
//             transform.LookAt(forward);
//             transform.Rotate(Vector3.right, -90f);
//         }
//     }
// }
