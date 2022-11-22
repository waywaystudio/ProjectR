using UnityEngine;

namespace Common.Character.Monster
{
    public class MonsterBehavior : MonoBehaviour
    {
        // Operation
        [SerializeField] private CharacterPathfinding characterPathfinding;
        
        // Graphic

        private void Awake()
        {
            characterPathfinding ??= GetComponentInChildren<CharacterPathfinding>();
            characterPathfinding.Initialize(null);
        }
    }
}
