using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Lobby
{
    public class LobbyCameraDirector : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private CinemachineVirtualCamera playerCamera;

        private CinemachineBrain cameraBrain;

        private void Awake()
        {
            cameraBrain = mainCamera.GetComponent<CinemachineBrain>();
        }

        public void SetPlayerCameraFocus(Transform target)
        {
            // playerCamera.Follow = target;
            // playerCamera.LookAt = target;
        }
        
        public void ChangeCamera(ICinemachineCamera cameraName)
        {
            var currentCamera = cameraBrain.ActiveVirtualCamera;
            if (currentCamera.Equals(cameraName)) return;

            (currentCamera.Priority, cameraName.Priority) = (cameraName.Priority, currentCamera.Priority);
        }
        
        [Button] private void PlayerCamera() => ChangeCamera(playerCamera);
    }
}
