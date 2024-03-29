using Adventurers;
using Character.Venturers;
using Character.Villains;
using Cinemachine;
using Manager;
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
            cameraBrain                  = mainCamera.GetComponent<CinemachineBrain>();
            MainManager.Input.MainCamera = mainCamera;
        }

        /* GameEvent */
        public void FocusPlayer(VenturerBehaviour target)
        {
            if (target.IsNullOrDestroyed()) return;
            
            Focusing(target.transform);
        }
        public void FocusMonster(VillainBehaviour target) => Focusing(target.transform);
        public void StageCamera() => ChangeCamera(stageCamera);

        public void ChangeCamera(ICinemachineCamera cameraName)
        {
            var currentCamera = cameraBrain.ActiveVirtualCamera;
            if (currentCamera.Equals(cameraName)) return;

            (currentCamera.Priority, cameraName.Priority) = (cameraName.Priority, currentCamera.Priority);
        }
        
        
        private void Focusing(Transform target)
        {
            if (!target.IsNullOrEmpty())
            {
                playerCamera.Follow = target;
                playerCamera.LookAt = target;
            }

            PlayerCamera();
        }

        private void PlayerCamera() => ChangeCamera(playerCamera);
        
    }
}
