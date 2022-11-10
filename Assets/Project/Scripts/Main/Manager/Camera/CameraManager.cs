using Sirenix.OdinInspector;
using UnityEngine;
using UnityCamera = UnityEngine.Camera;

namespace Main.Camera
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private Transform followTarget;
        [SerializeField] private Vector3 positionOffset = new (0, 25, -25);
        [SerializeField] private Vector3 rotationOffset = new (45, 0, 0);
        
        [ShowInInspector]
        private UnityCamera mainCamera;
        [ShowInInspector]
        private Transform mainCameraObject;
        
        public UnityCamera MainCamera => mainCamera ??= UnityCamera.main;
        public Transform MainCameraObject => mainCameraObject ??= MainCamera.transform;

        public void SetFollow(Transform target) => followTarget = target;
        public void ReleaseFollow() => followTarget = null;

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
