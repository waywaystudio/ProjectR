using DG.Tweening;
using Main;
using UnityEngine;

namespace Raid
{
    public class RaidDirector : MonoBehaviour
    {
        [SerializeField] private RaidCameraDirector cameraDirector;

        private void Awake()
        {
            cameraDirector ??= GetComponent<RaidCameraDirector>();
            
            
            MainGame.UI.FadePanel.PlayFadeIn();
        }
    }
}
