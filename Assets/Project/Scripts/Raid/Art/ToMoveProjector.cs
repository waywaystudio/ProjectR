using UnityEngine;

namespace Raid.Art
{
    public class ToMoveProjector : MonoBehaviour
    {
        [SerializeField] private ProjectorController controller;

        public void Projecting()
        {
            if (controller.IsNullOrDestroyed()) return;
            
            if (!InputManager.TryGetMousePosition(out var groundPosition)) return;

            transform.position = groundPosition;
            controller.FillOnce(0.3f);
        }


        private void Awake()
        {
            controller ??= GetComponentInChildren<ProjectorController>();
        }
    }
}
