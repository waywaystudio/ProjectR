using Cinemachine;
using Manager;
using UnityEngine;

namespace Lobby
{
    public class LobbyCameraDirector : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private CinemachineVirtualCamera playerCamera;

        private CinemachineBrain cameraBrain;


        public void Initialize(Transform adventurer)
        {
            cameraBrain                  = mainCamera.GetComponent<CinemachineBrain>();
            MainManager.oldInput.MainCamera = mainCamera;
            
            playerCamera.Follow = adventurer;
            playerCamera.LookAt = adventurer;
            
            // ChangeCamera(playerCamera);
        }
        
        public void ChangeCamera(ICinemachineCamera cameraName)
        {
            Debug.Log(cameraBrain != null);
            Debug.Log(cameraBrain.ActiveVirtualCamera != null);
            Debug.Log(cameraName);
            
            var currentCamera = cameraBrain.ActiveVirtualCamera;
            if (currentCamera.Equals(cameraName)) return;

            (currentCamera.Priority, cameraName.Priority) = (cameraName.Priority, currentCamera.Priority);
        }

        // public void ChangeCamera(ICinemachineCamera cameraName)
        // {
        //     var currentCamera = cameraBrain.ActiveVirtualCamera;
        //     if (currentCamera.Equals(cameraName)) return;
        //
        //     (currentCamera.Priority, cameraName.Priority) = (cameraName.Priority, currentCamera.Priority);
        // }
        //
        // [Button] private void PlayerCamera() => ChangeCamera(playerCamera);
    }
}
