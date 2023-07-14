using UnityEngine;

namespace Raid.Art
{
    public class ToAvoidProjector : MonoBehaviour
    {
        [SerializeField] private ProjectorController controller;

        private void Awake()
        {
            controller ??= GetComponentInChildren<ProjectorController>();
        }
    }
}
