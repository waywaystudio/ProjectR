using UnityEngine;

namespace Lobby
{
    public class LobbyDirector : MonoBehaviour
    {
        [SerializeField] private LobbyCameraDirector cameraDirector;
        

        private void Awake()
        {
            cameraDirector ??= GetComponentInChildren<LobbyCameraDirector>();
            // cameraDirector.SetPlayerCameraFocus(player.transform);
            
            MainUI.FadePanel.PlayFadeIn();
        }
    }
}
