using UnityEngine;

namespace Town
{
    public class TownBehaviour : MonoBehaviour
    {
        [SerializeField] private TownCameraDirector cameraDirector;
        [SerializeField] private PlayerController player;

        public TownCameraDirector CameraDirector => cameraDirector;
        public PlayerController Player => player;

        private void Awake()
        {
            CameraDirector.FollowTarget = Player.transform;
        }
    }
}
