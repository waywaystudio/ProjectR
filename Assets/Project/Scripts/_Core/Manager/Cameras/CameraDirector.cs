using Cinemachine;
using UnityEngine;

namespace Cameras
{
    public class CameraDirector : MonoBehaviour
    {
        [SerializeField] protected Camera mainCamera;
        [SerializeField] protected CinemachineBrain brain;
        [SerializeField] protected Table<VirtualCameraType, CinemachineVirtualCamera> subCameraTable;

        public Camera MainCamera => mainCamera;
        public CinemachineBrain Brain => brain;
        
        public CinemachineVirtualCamera this[VirtualCameraType key]
        {
            get => subCameraTable[key];
            set => subCameraTable[key] = value;
        }
        
        public VirtualCameraType CurrentCameraType { get; protected set; }

        public void Initialize()
        {
            CameraManager.SetDirector(this);
        }
        
        public void ChangeCamera(VirtualCameraType cameraType)
        {
            var activeVirtualCamera = brain.ActiveVirtualCamera;

            if (activeVirtualCamera is null)
            {
                Debug.LogWarning("VirtualCamera None");
                return;
            }
            
            var activeCamera = activeVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
            if (activeCamera.Equals(this[cameraType])) return;
        
            (activeCamera.Priority, this[cameraType].Priority) 
                = (this[cameraType].Priority, activeCamera.Priority);
            CurrentCameraType = cameraType;
        }


        protected void OnDestroy()
        {
            CameraManager.Director = null;
        }
    }
}
