using Character;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Raid
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

        public void SetPlayerCameraFocus(AdventurerBehaviour target) => SetPlayerCameraFocus(target.transform);

        public void ChangeCamera(ICinemachineCamera cameraName)
        {
            var currentCamera = cameraBrain.ActiveVirtualCamera;
            if (currentCamera.Equals(cameraName)) return;

            (currentCamera.Priority, cameraName.Priority) = (cameraName.Priority, currentCamera.Priority);
        }
        
        
        private void SetPlayerCameraFocus(Transform target)
        {
            playerCamera.Follow = target;
            playerCamera.LookAt = target;

            PlayerCamera();
        }

        [Button] private void PlayerCamera() => ChangeCamera(playerCamera);
        [Button] private void StageCamera() => ChangeCamera(stageCamera);
    }
}
