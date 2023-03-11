using Manager;
using UnityEngine;

namespace Raid.Art
{
    public class ToAvoidProjector : MonoBehaviour
    {
        [SerializeField] private ProjectorController controller;

        public void Projecting()
        {
            if (!MainManager.Input.TryGetGroundPosition(out var groundPosition)) return;

            transform.position = groundPosition;
            controller.FillOnce(0.3f);
        }


        private void Awake()
        {
            controller ??= GetComponentInChildren<ProjectorController>();
        }
    }
}
