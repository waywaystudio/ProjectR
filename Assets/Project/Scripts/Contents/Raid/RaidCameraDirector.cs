using System.Collections.Generic;
using Cinemachine;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Raid
{
    public class RaidCameraDirector : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private CinemachineVirtualCamera playerCamera;
        [SerializeField] private CinemachineVirtualCamera stageCamera;

        private CinemachineBrain cameraBrain;
        private Dictionary<string, ICinemachineCamera> cameraTable = new ();

        private void Awake()
        {
            mainCamera ??= GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            cameraBrain = mainCamera.GetComponent<CinemachineBrain>();

            GetComponentsInChildren<ICinemachineCamera>().ForEach(x => cameraTable.Add(x.Name, x));
        }

        public void SetPlayerCameraFocus(Transform target)
        {
            playerCamera.Follow = target;
            playerCamera.LookAt = target;
        }

        public void ChangeCamera(string cameraName)
        {
            var currentCamera = cameraBrain.ActiveVirtualCamera;

            if (currentCamera.Name.Equals(cameraName)) return;
            if (!cameraTable.TryGetValue(cameraName, out var targetCamera))
            {
                Debug.LogError($"Not Exist {cameraName} in {GetType().Name}");
                return;
            }

            (currentCamera.Priority, targetCamera.Priority) = (targetCamera.Priority, currentCamera.Priority);
        }

        [Button]
        private void PlayerCamera() => ChangeCamera("PlayerCamera");
        
        [Button]
        private void StageCamera() => ChangeCamera("StageCamera");
    }
}
