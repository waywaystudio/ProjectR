using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Character
{
    public class CharacterDirectionGuide : MonoBehaviour
    {
        public void MatchForward(Vector3 direction)
        {
            if (direction.Equals(Vector3.zero)) return;
            
            var forward = transform.position + direction;

            transform.LookAt(forward);
            transform.Rotate(Vector3.right, -90f);
        }
    }
}
