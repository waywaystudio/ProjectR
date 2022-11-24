using System;
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
            characterPathfinding.Initialize();
        }

        private void Walk(Vector3 destination)
        {
            characterPathfinding.Move(destination, 10f);
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;
        
            // ReSharper disable once Unity.PerformanceCriticalCodeCameraMain
            // ReSharper disable once PossibleNullReferenceException
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
            if (!Physics.Raycast(ray, out var hit)) return;

            Walk(hit.point);
        }
    }
}
