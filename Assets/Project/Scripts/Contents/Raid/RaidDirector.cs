using MainGame;
using UnityEngine;

namespace Raid
{
    public class RaidDirector : MonoBehaviour
    {
        [SerializeField] private RaidCameraDirector cameraDirector;
        [SerializeField] private RaidStageDirector stageDirector;

        private void Awake()
        {
            cameraDirector ??= GetComponentInChildren<RaidCameraDirector>();
            stageDirector ??= GetComponentInChildren<RaidStageDirector>();

            MainUI.FadePanel.PlayFadeIn();
        }
    }
}
