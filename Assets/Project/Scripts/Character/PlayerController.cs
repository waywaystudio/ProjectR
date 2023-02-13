using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private InputAction mouseClick;
        [SerializeField] private LayerMask groundLayer;
        // [SerializeField] private CharacterBehaviour cb;

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

                // cb.Run(destination, () => Debug.Log("Movement Callback"));
            }
        }
    }
}
