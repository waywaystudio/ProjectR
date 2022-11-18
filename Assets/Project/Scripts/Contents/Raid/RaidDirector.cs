using DG.Tweening;
using Main;
using UnityEngine;

namespace Raid
{
    public class RaidDirector : MonoBehaviour
    {
        [SerializeField] private RaidCameraDirector cameraDirector;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Transform hierarchy;
        [SerializeField] private Transform spawnPoint;

        private void Awake()
        {
            cameraDirector ??= GetComponent<RaidCameraDirector>();
            
            var player = Instantiate(playerPrefab, hierarchy);
            var position = spawnPoint.position;
            var rotation = spawnPoint.rotation;
            
            player.transform.SetPositionAndRotation(position, rotation);

            cameraDirector.SetPlayerCameraFocus(player.transform);
            MainGame.UI.FadePanel.PlayFadeIn();
        }
    }
}
