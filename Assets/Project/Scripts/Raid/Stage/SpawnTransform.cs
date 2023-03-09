using System;
using UnityEngine;

namespace Raid.Stage
{
    public class SpawnTransform : MonoBehaviour
    {
        [SerializeField] private string iconName;
        [SerializeField] private Color color;

        public string IconName => iconName;

        private void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position + Vector3.up * 1f, $"{iconName}.png", true, color);
        }
    }
}
