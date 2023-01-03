using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scene.Raid
{
    public class RaidCameraDirector : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private CinemachineVirtualCamera playerCamera;
        [SerializeField] private CinemachineVirtualCamera stageCamera;

        private CinemachineBrain cameraBrain;

        private void Awake()
        {
            cameraBrain = mainCamera.GetComponent<CinemachineBrain>();
        }

        public void SetPlayerCameraFocus(Transform target)
        {
            playerCamera.Follow = target;
            playerCamera.LookAt = target;

            PlayerCamera();
        }

        public void ChangeCamera(ICinemachineCamera cameraName)
        {
            var currentCamera = cameraBrain.ActiveVirtualCamera;
            if (currentCamera.Equals(cameraName)) return;

            (currentCamera.Priority, cameraName.Priority) = (cameraName.Priority, currentCamera.Priority);
        }

        [Button] private void PlayerCamera() => ChangeCamera(playerCamera);
        [Button] private void StageCamera() => ChangeCamera(stageCamera);
    }
}
