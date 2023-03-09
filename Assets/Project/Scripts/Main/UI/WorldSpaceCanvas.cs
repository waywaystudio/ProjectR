using UnityEngine;

namespace UI
{
    public class WorldSpaceCanvas : MonoBehaviour
    {
        private void Awake()
        {
            var canvas = GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
        }
    }
}