using System.Linq;
using Cameras;
using Character.Venturers;
using Cinemachine;
using UnityEngine;

namespace Raid
{
    public class RaidCameraDirector : CameraDirector, IEditable
    {
        public void OnFocusVenturerChanged(VenturerBehaviour vb)
        {
            if (vb.IsNullOrDestroyed()) return;
            
            Focusing(vb.transform);
        }

        public void OnCommandMode()
        {
            ChangeCamera(VirtualCameraType.Stage);
        }


        private void Focusing(Transform target)
        {
            if (!target.IsNullOrEmpty())
            {
                subCameraTable[VirtualCameraType.Player].Follow   = target;
                subCameraTable[VirtualCameraType.Player].LookAt   = target;
                subCameraTable[VirtualCameraType.Effector].Follow = target;
                subCameraTable[VirtualCameraType.Effector].LookAt = target;
            }

            ChangeCamera(VirtualCameraType.Player);
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
