using System;
using Character.Graphic;
using Core;
using DG.Tweening;
using Pathfinding;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.Movement
{
    public class MovementBehaviour : MonoBehaviour, IPathfinding
    {
        [SerializeField] private Transform rootTransform; 
        [SerializeField] private Seeker agent;
        [SerializeField] private AIMove aiMove;
        [SerializeField] private AnimationModel model;
        [SerializeField] private float moveSpeed;
        [SerializeField] private LayerMask environmentLayer;
        [SerializeField] private LayerMask groundLayer;

        public bool IsReached => aiMove.reachedEndOfPath;
        public bool IsSafe => PathfindingUtility.IsSafePosition(rootTransform.position);
        public Vector3 Direction => rootTransform.forward;

        private Vector3 rootPosition => rootTransform.position;
        private readonly RaycastHit[] rayBuffers = new RaycastHit[32];

        public void Move(Vector3 destination, Action callback)
        {
            aiMove.maxSpeed = moveSpeed;

            if (callback != null)
            {
                aiMove.Callback -= callback;
                aiMove.Callback += callback;
            }

            agent.StartPath(rootPosition, destination);
            
            // + RunAnimation
            model.Run();
        }
        
        public void Rotate(Vector3 lookTarget) => rootTransform.LookAt(lookTarget);

        public void MoveStop()
        {
            aiMove.SetPath(null);
            
            // + IdleAnimation
            model.Idle();
        }

        public void Dash()
        {
            if (!TryGetMousePosition(out var mousePosition)) return;

            Dash(mousePosition - rootPosition);
        }
        
        public void Dash(Vector3 direction)
        {
            MoveStop();

            var normalDirection = direction.normalized;
            var dashDestination = rootPosition + normalDirection * 8f;
            var distance = Vector3.Distance(dashDestination, rootPosition);
            
            if (Physics.Raycast(rootPosition, normalDirection, out var hitInfo, distance, environmentLayer))
            {
                // TODO. 0.25f to ThreshHold
                distance = hitInfo.distance - 1f;
                
                Debug.Log($"Hit Something. {hitInfo.transform.name}");
            }

            rootTransform.DOMove(rootPosition + normalDirection * distance, 0.15f).OnComplete(model.Idle);
            
            // + DashAnimation
            model.PlayOnce("jump");
        }

        public void Teleport()
        {
            if (!TryGetMousePosition(out var mousePosition)) return;
            
            Teleport(mousePosition - rootPosition);
        }
        
        public void Teleport(Vector3 direction)
        {
            MoveStop();

            var normalDirection = direction.normalized;
            var teleportDestination = rootPosition + normalDirection * 8f;
            var distance = Vector3.Distance(teleportDestination, rootPosition);

            if (!PathfindingUtility.IsSafePosition(teleportDestination))
            {
                if (Physics.Raycast(rootPosition, normalDirection, out var hitInfo, distance, environmentLayer))
                {
                    // TODO. 0.25f to ThreshHold
                    distance            = hitInfo.distance - 1f;
                    teleportDestination = rootPosition + normalDirection * distance;
                }
                else
                {
                    teleportDestination = PathfindingUtility.GetNearestSafePathNode(rootTransform.position, teleportDestination);
                }
            }
            
            aiMove.Teleport(teleportDestination);
            
            // + IdleAnimation
            model.Idle();
        }

        [Button]
        public void KnockBack() => KnockBack(Vector3.zero);
        
        // KnockBack
        public void KnockBack(Vector3 from)
        {
            MoveStop();
            Rotate(from);
            
            var knockBackDirection = (rootPosition - from).normalized;
            var knockBackDestination = rootPosition + knockBackDirection * 5f;
            var distance = Vector3.Distance(knockBackDestination, rootPosition);
            
            if (Physics.Raycast(rootPosition, knockBackDestination, out var hitInfo, distance, environmentLayer))
            {
                // TODO. 0.25f to ThreshHold
                distance = hitInfo.distance - 1f;
            }

            rootTransform.DOMove(rootPosition + knockBackDirection * distance, 0.15f).OnComplete(model.Idle);
            
            model.PlayOnce("fall");
        }

        private bool TryGetMousePosition(out Vector3 mousePosition)
        {
            mousePosition = Vector3.negativeInfinity;
                
            var plane = new Plane(Vector3.up, 0f);
            var ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!plane.Raycast(ray, out var distance)) return false;
            
            mousePosition = ray.GetPoint(distance);
            return true;
        }

        private void Update()
        {
            model.Flip(Direction);
        }

        #region TESTFIELD

        [SerializeField] private InputAction mouseClick;
        [SerializeField] private InputAction dashInput;
        [SerializeField] private InputAction teleportInput;
        
        private Camera mainCamera;
        private Vector3 mousedDestination = Vector3.zero;
        
        private void OnEnable()
        {
            mouseClick.Enable();
            dashInput.Enable();
            teleportInput.Enable();
            
            mainCamera              =  Camera.main;
            mouseClick.performed    += OnMove;
            dashInput.performed     += OnDash;
            teleportInput.performed += OnTeleport;
        }

        private void OnDisable()
        {
            mouseClick.performed    -= OnMove;
            dashInput.performed     -= OnDash;
            teleportInput.performed -= OnTeleport;
            
            mouseClick.Disable();
            dashInput.Disable();
            teleportInput.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            var ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray: ray, hitInfo: out var hit) 
                && hit.collider 
                // && hit.collider.gameObject.IsInLayerMask(groundLayer)
               )
            {
                mousedDestination.x = hit.point.x;
                mousedDestination.z = hit.point.z;

                Move(mousedDestination, model.Idle);
            }
        }

        public void OnDash(InputAction.CallbackContext context) => Dash();
        public void OnTeleport(InputAction.CallbackContext context) => Teleport();

        #endregion
    }
}
