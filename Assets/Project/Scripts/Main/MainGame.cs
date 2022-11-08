using Main.Manager.Control;
using Sirenix.OdinInspector;
using UnityEngine;
using Wayway.Engine.Audio;
using Wayway.Engine.Save;
using Wayway.Engine.Singleton;

namespace Main
{
    public class MainGame : MonoSingleton<MainGame>
    {
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private ControlManager controlManager;

        public static AudioManager AudioManager => Instance.audioManager ??= Instance.GetComponentInChildren<AudioManager>();
        public static SaveManager SaveManager => Instance.saveManager ??= Instance.GetComponentInChildren<SaveManager>();
        public static ControlManager ControlManager => Instance.controlManager ??= Instance.GetComponentInChildren<ControlManager>();

        protected override void Awake()
        {
            base.Awake();

            Application.targetFrameRate = 60;
        }

        [Button]
        public void Function()
        {
            Debug.Log("Function On");
        }
    }
}
