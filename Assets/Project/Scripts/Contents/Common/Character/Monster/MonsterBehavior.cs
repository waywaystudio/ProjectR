using UnityEngine;

namespace Common.Character.Monster
{
    public class MonsterBehavior : MonoBehaviour
    {
        // Operation
        [SerializeField] private CharacterPathfinding characterPathfinding;
        
        // Graphic
        [SerializeField] private CharacterDirectionGuide directionGuide;
        
        private void Awake()
        {
            characterPathfinding ??= GetComponentInChildren<CharacterPathfinding>();
            characterPathfinding.Initialize(5, null);
            
            directionGuide ??= GetComponentInChildren<CharacterDirectionGuide>();
        }
        
        private void Start()
        {
            
        }
    }
}
