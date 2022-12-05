using MainGame;
using UnityEngine;

namespace Raid
{
    public class RaidDirector : MonoBehaviour
    {
        [SerializeField] private RaidCameraDirector cameraDirector;

        private void Awake()
        {
            cameraDirector ??= GetComponent<RaidCameraDirector>();

            MainUI.FadePanel.PlayFadeIn();
        }
    }
}
