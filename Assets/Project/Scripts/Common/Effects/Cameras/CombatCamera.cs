using Cinemachine;
using UnityEngine;
using Camera = UnityEngine.Camera;

namespace Common.Effects.Cameras
{
    public class CombatCamera : MonoBehaviour
    {
        [SerializeField] private Section playSection;
        [SerializeField] private Section endSection;
        [SerializeField] private float magnification = 1f;
        [SerializeField] private ZoomType zoomType;
        
        private CinemachineVirtualCamera activeVCam;
        private float originalFov;
        private Camera mainCamera;
        private CinemachineBrain brain;

        private Camera MainCamera => mainCamera ??= Camera.main;
        private CinemachineBrain Brain => brain 
            ? brain 
            : brain = MainCamera.GetComponent<CinemachineBrain>();
        private CinemachineVirtualCamera ActiveCam => Brain.ActiveVirtualCamera as CinemachineVirtualCamera;

        private enum ZoomType { None = 0, In, Out }
        

        public void PlayZoom()
        {
            // Current Camera != PlayerCamera return;
            
        }

        public void ReturnZoom()
        {
            
        }
        

        public void Awake()
        {
            activeVCam = Brain.ActiveVirtualCamera as CinemachineVirtualCamera;

            if (activeVCam != null)
            {
                originalFov = activeVCam.m_Lens.FieldOfView;
                // activeVCam.m_Lens.FieldOfView = 0f;
            }
        }


        public void Test()
        {
            Debug.Log(ActiveCam.name);
        }
    }
}
