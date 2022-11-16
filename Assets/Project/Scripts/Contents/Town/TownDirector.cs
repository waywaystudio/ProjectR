using Main;
using UnityEngine;

namespace Town
{
    public class TownDirector : MonoBehaviour
    {
        [SerializeField] private TownCameraDirector cameraDirector;
        
        // Temp
        [SerializeField] private Transform player;

        private void Awake()
        {
            cameraDirector ??= GetComponentInChildren<TownCameraDirector>();
            cameraDirector.SetPlayerCameraFocus(player.transform);
            
            MainGame.UI.FadePanel.PlayFadeIn();
        }
    }
}