using Core;
using UnityEngine;

namespace Common.Character.Monster
{
    public class MonsterBehavior : MonoBehaviour, ICombatTaker
    {
        // Operation
        [SerializeField] private Operation.Pathfinding Pathfinding;
        
        // Graphic
        
        public void Walk(Vector3 destination)
        {
            // characterPathfinding.OLD_Move(destination, moveSpeed);
        }
        
        private void Awake()
        {
            Pathfinding ??= GetComponentInChildren<Operation.Pathfinding>();
            // characterPathfinding.Initialize();
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;
        
            // ReSharper disable once Unity.PerformanceCriticalCodeCameraMain
            // ReSharper disable once PossibleNullReferenceException
            #region TEST FUNCTION

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
            if (!Physics.Raycast(ray, out var hit)) return;

            Walk(hit.point);

            #endregion
        }

        public GameObject Taker => gameObject;
        public void TakeDamage(IDamageProvider damageInfo)
        {
            Debug.Log("TakeDamage!");
        }

        public void TakeHeal(IHealProvider healInfo)
        {
            Debug.Log("TakeHeal!");
        }

        public void TakeExtra(IExtraProvider extra)
        {
            Debug.Log("TakeExtra!");
        }
    }
}
