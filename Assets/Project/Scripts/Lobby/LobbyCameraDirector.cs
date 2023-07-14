using System.Linq;
using Cameras;
using Character.Venturers;
using Cinemachine;
using UnityEngine;

namespace Lobby
{
    public class LobbyCameraDirector : CameraDirector, IEditable
    {
        public void OnFocusVenturerChanged(VenturerBehaviour target)
        {
            if (target.IsNullOrDestroyed()) return;
            
            Focusing(target.transform);
        }
        
        public void StageCamera() => ChangeCamera(VirtualCameraType.Stage);
        public void PlayerCamera() => ChangeCamera(VirtualCameraType.Player);
        
        
        private void Focusing(Transform target)
        {
            if (!target.IsNullOrEmpty())
            {
                subCameraTable[VirtualCameraType.Player].Follow = target;
                subCameraTable[VirtualCameraType.Player].LookAt = target;
            }

            PlayerCamera();
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
