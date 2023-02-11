using System;
using Character.Graphic;
using Core;
using Pathfinding;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.Movement
{
    /*
     * 아주 기초적인 움직임을 다룰수는 있겠다.
     * 이동, 달리기, 멈추기, 쓰러지기, 텔레포트 등.
     * Pathfinding 기능을 사용해서 이곳에서 다 처리 할지, 클래스를 나눌지 결정해야 하는데
     * 현재 기조상, 이곳에서 처리해보고자 한다.
     */
    
    public class MovementBehaviour : MonoBehaviour, IPathfinding
    {
        [SerializeField] private Transform rootTransform; 
        [SerializeField] private Seeker agent;
        [SerializeField] private AIMove aiMove;
        [SerializeField] private AnimationModel model;
        [SerializeField] private float moveSpeed;
        
        private ABPath pathBuffer;
        
        public bool IsReached => aiMove.reachedEndOfPath;
        public bool IsSafe => PathfindingUtility.IsSafePosition(rootTransform.position);
        public Vector3 Direction { get; }
        
        // aiMove Sequence
        // Can move, or whatever
        [ShowInInspector] public BoolTable ConditionTable { get; } = new();

        [ShowInInspector] public ActionTable OnActivated { get; } = new();
        [ShowInInspector] public ActionTable OnInterrupted { get; } = new();
        [ShowInInspector] public ActionTable OnHit { get; } = new();
        [ShowInInspector] public ActionTable OnCompleted { get; } = new();
        [ShowInInspector] public ActionTable OnEnded { get; } = new();

        
        public void Move(Vector3 destination, Action callback = null)
        {
            pathBuffer      = ABPath.Construct(rootTransform.position, destination);
            aiMove.maxSpeed = moveSpeed;

            if (callback != null) aiMove.Callback += callback;
            
            agent.StartPath(pathBuffer);
            
            // + RunAnimation
            model.Play("run");
        }

        public void Stop()
        {
            pathBuffer = null;
            aiMove.SetPath(null);
            
            // + IdleAnimation
            model.Idle();
        }
        
        public void Teleport(Vector3 destination)
        {
            var safeDestination = destination;

            if (!PathfindingUtility.IsSafePosition(destination))
            {
                safeDestination = PathfindingUtility.GetNearestSafePathNode(rootTransform.position, destination);
            }
            
            aiMove.Teleport(safeDestination);
            
            // + IdleAnimation
            model.Idle();
        }

        // Dash
        // KnockBack
        // Pushing

        #region TESTFIELD

        [SerializeField] private InputAction mouseClick;
        
        private Camera mainCamera;
        private Vector3 destination = Vector3.zero;
        
        private void OnEnable()
        {
            mouseClick.Enable();
            mouseClick.performed += OnMove;
            mainCamera           =  Camera.main;
        }

        private void OnDisable()
        {
            mouseClick.performed -= OnMove;
            mouseClick.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            var ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray: ray, hitInfo: out var hit) 
                && hit.collider 
                // && hit.collider.gameObject.IsInLayerMask(groundLayer)
               )
            {
                destination.x = hit.point.x;
                destination.z = hit.point.z;

                Move(destination, () => Debug.Log("Movement Callback"));
            }
        }

        #endregion
    }
}
