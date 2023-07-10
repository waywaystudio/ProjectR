using Character.Venturers;
using Cinemachine;
using Manager;
using UnityEngine;

namespace Raid
{
    public class RaidCameraDirector : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private CinemachineBrain cameraBrain;
        [SerializeField] private Table<int, CinemachineVirtualCamera> subCameraTable;
        [SerializeField] private CinemachineVirtualCamera playerCamera;
        [SerializeField] private CinemachineVirtualCamera stageCamera;

        public CinemachineVirtualCamera CurrentCamera { get; set; }


        /* GameEvent */
        public void FocusPlayer(VenturerBehaviour target)
        {
            if (target.IsNullOrDestroyed()) return;
            
            Focusing(target.transform);
        }
        
        public void StageCamera() => ChangeCamera(stageCamera);
        public void PlayerCamera() => ChangeCamera(playerCamera);

        public void ChangeCamera(CinemachineVirtualCamera camera)
        {
            var activeCamera = cameraBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
            if (activeCamera.Equals(camera)) return;
        
            activeCamera.Priority = 10;
            camera.Priority       = 20;
            CurrentCamera         = camera;
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

        private void Awake()
        {
            MainManager.Input.MainCamera = mainCamera;
        }
    }
}
