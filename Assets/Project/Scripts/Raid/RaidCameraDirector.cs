using System.Linq;
using Cameras;
using Character.Venturers;
using Cinemachine;
using Manager;
using UnityEngine;

namespace Raid
{
    public class RaidCameraDirector : CameraDirector, IEditable
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private CinemachineBrain cameraBrain;


        /* GameEvent */
        public void FocusPlayer(VenturerBehaviour target)
        {
            if (target.IsNullOrDestroyed()) return;
            
            Focusing(target.transform);
        }
        
        public void StageCamera() => ChangeCamera(VirtualCameraType.Stage);
        public void PlayerCamera() => ChangeCamera(VirtualCameraType.Player);

        public void ChangeCamera(VirtualCameraType cameraType)
        {
            var activeCamera = cameraBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
            if (activeCamera.Equals(this[cameraType])) return;
        
            activeCamera.Priority     = 10;
            this[cameraType].Priority = 20;
            CurrentCameraType         = cameraType;
        }


        private void Focusing(Transform target)
        {
            if (!target.IsNullOrEmpty())
            {
                subCameraTable[VirtualCameraType.Player].Follow = target;
                subCameraTable[VirtualCameraType.Player].LookAt = target;
            }

            PlayerCamera();
        }

        private void Awake()
        {
            MainManager.Input.MainCamera = mainCamera;
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            subCameraTable.Clear();

            var virtualCameraList = GetComponentsInChildren<CinemachineVirtualCamera>().ToList();
            
            virtualCameraList.ForEach(vc =>
            {
                subCameraTable.Add(vc.gameObject.name.Replace("Camera", "").ToEnum<VirtualCameraType>(), vc);
            });
        }
#endif
    }
}
