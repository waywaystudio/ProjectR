using Cinemachine;
using UnityEngine;

namespace Cameras
{
    public class CameraDirector : MonoBehaviour
    {
        [SerializeField] protected Camera mainCamera;
        [SerializeField] protected CinemachineBrain cameraBrain;
        [SerializeField] protected Table<VirtualCameraType, CinemachineVirtualCamera> subCameraTable;
        
        public CinemachineVirtualCamera this[VirtualCameraType key]
        {
            get => subCameraTable[key];
            set => subCameraTable[key] = value;
        }
        
        public VirtualCameraType CurrentCameraType { get; protected set; }
        
        public void ChangeCamera(VirtualCameraType cameraType)
        {
            var activeCamera = cameraBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
            if (activeCamera.Equals(this[cameraType])) return;
        
            (activeCamera.Priority, this[cameraType].Priority) 
                = (this[cameraType].Priority, activeCamera.Priority);
            CurrentCameraType = cameraType;
        }
    }
}
