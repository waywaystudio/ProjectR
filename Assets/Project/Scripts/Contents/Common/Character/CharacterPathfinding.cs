using System.Linq;
using Pathfinding;
using Pathfinding.RVO;
using UnityEngine;

namespace Common.Character
{
    public class CharacterPathfinding : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private int fleeLength = 500;
        [SerializeField] private int fleeSpread = 500;
        [SerializeField] private float fleeAimStrength = 1f;
        [SerializeField] private Transform rootObject;
        [SerializeField] private Rigidbody rigid;
        [SerializeField] private Transform testTarget;

        private Seeker agent;
        private AIMove aiMove;

        public bool CanMove
        {
            get => aiMove.canMove;
            set => aiMove.canMove = value;
        }
        public Vector3 Destination
        {
            get => aiMove.destination;
            set => aiMove.destination = value;
        }
        public Vector3 Direction => aiMove.desiredVelocity.normalized;
        public float MoveSpeed => aiMove.maxSpeed;

        public void MoveTo(Vector3 position)
        {
            Destination = position;
        }

        public void AvoidFrom(Vector3 position)
        {
            var flee = FleePath.Construct(rootObject.position, position, fleeLength, GetAvoidPosition);
            flee.aimStrength = fleeAimStrength;
            flee.spread = fleeSpread;

            agent.StartPath(flee);
        }

        public void ScrumTo(Vector3 position)
        {
            
        }

        private void Awake()
        {
            if (rootObject is null || rigid is null)
            {
                Debug.LogError("Pathfinding Field is Null");
            }
            
            agent ??= GetComponent<Seeker>();
            aiMove ??= GetComponent<AIMove>();
            aiMove.Initialize(rootObject, agent, rigid);
            aiMove.maxSpeed = moveSpeed;
        }

        private void GetAvoidPosition(Path p)
        {
            Destination = p.vectorPath.Last();
        }

        #region TESTFIELD
        private Camera mainCamera;
    
        private void Start () 
        {
            mainCamera = Camera.main;
        }
    
        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;
        
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
            if (!Physics.Raycast(ray, out var hit)) return;

            testTarget.position = hit.point;

            MoveTo(testTarget.position);
        }

        #endregion
    }
}
