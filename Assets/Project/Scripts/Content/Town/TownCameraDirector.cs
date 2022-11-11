using Sirenix.OdinInspector;
using UnityEngine;
using UnityCamera = UnityEngine.Camera;

namespace Town
{
    public class TownCameraDirector : MonoBehaviour
    {
        [SerializeField] private Transform followTarget;
        [SerializeField] private Vector3 positionOffset = new (0, 15, -15);
        [SerializeField] private Vector3 rotationOffset = new (45, 0, 0);

        private bool isFollowing;
        
        [ShowInInspector, ReadOnly] private UnityCamera mainCamera;
        [ShowInInspector, ReadOnly] private Transform mainCameraObject;
        
        public UnityCamera MainCamera => MainCameraObject.GetComponent<UnityCamera>();
        public Transform MainCameraObject => mainCameraObject ??= GameObject.FindWithTag("MainCamera").transform;

        public Transform FollowTarget
        {
            set
            {
                followTarget = value;
                isFollowing = value is not null;

                if (!isFollowing)
                    SetDefaultTransform();
            }
        }

        private void Awake()
        {
            mainCameraObject = GameObject.FindWithTag("MainCamera").transform;
            mainCamera = mainCameraObject.GetComponent<UnityCamera>();
        }

        private void Update()
        {
            if (!isFollowing || !MainCameraObject.hasChanged) 
                return;
            
            var trackingPosition = followTarget.position + positionOffset;
            var trackingRotation = followTarget.rotation.eulerAngles + rotationOffset;
            var trackingQuaternion = Quaternion.Euler(trackingRotation);
                
            MainCameraObject.SetPositionAndRotation(trackingPosition, trackingQuaternion);
        }

        private void SetDefaultTransform()
        {
            MainCameraObject.SetPositionAndRotation(positionOffset, Quaternion.Euler(rotationOffset));
        }
    }
}