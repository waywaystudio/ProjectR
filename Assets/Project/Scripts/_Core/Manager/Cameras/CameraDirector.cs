using Cinemachine;
using UnityEngine;

namespace Cameras
{
    public class CameraDirector : MonoBehaviour
    {
        [SerializeField] protected Table<VirtualCameraType, CinemachineVirtualCamera> subCameraTable;
        
        public CinemachineVirtualCamera this[VirtualCameraType key]
        {
            get => subCameraTable[key];
            set => subCameraTable[key] = value;
        }
        
        public VirtualCameraType CurrentCameraType { get; protected set; }
    }
}
