using UnityEngine;

namespace Main.Camera
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private Transform followTarget;
        [SerializeField] private Vector3 positionOffset = new (0, 25, -25);
        [SerializeField] private Vector3 rotationOffset = new (45, 0, 0);
        
        private UnityEngine.Camera mainCamera;
        private Transform mainCameraObject;
        
        public UnityEngine.Camera MainCamera => mainCamera ??= UnityEngine.Camera.main;
        public Transform MainCameraObject => mainCameraObject ??= MainCamera.transform;

        public void SetFollow(Transform target) => followTarget = target;

        private void Update()
        {
            if (followTarget is null || !MainCameraObject.hasChanged) return;
            
            var trackingPosition = followTarget.position + positionOffset;
            var trackingRotation = followTarget.rotation.eulerAngles + rotationOffset;
            var trackingQuaternion = Quaternion.Euler(trackingRotation);
                
            MainCameraObject.SetPositionAndRotation(trackingPosition, trackingQuaternion);
        }
    }
}
