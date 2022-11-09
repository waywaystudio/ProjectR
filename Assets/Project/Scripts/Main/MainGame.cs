using Main.Audio;
using Main.Input;
using Main.Camera;
using Main.Save;
using UnityEngine;
using Wayway.Engine.Singleton;

namespace Main
{
    public class MainGame : MonoSingleton<MainGame>
    {
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private CameraManager cameraManager;
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private InputManager inputManager;

        public static AudioManager AudioManager => Instance.audioManager ??= Instance.GetComponentInChildren<AudioManager>();
        public static CameraManager CameraManager => Instance.cameraManager ??= Instance.GetComponentInChildren<CameraManager>();
        public static SaveManager SaveManager => Instance.saveManager ??= Instance.GetComponentInChildren<SaveManager>();
        public static InputManager InputManager => Instance.inputManager ??= Instance.GetComponentInChildren<InputManager>();

        protected override void Awake()
        {
            base.Awake();

            Application.targetFrameRate = 60;
        }

        [Sirenix.OdinInspector.Button]
        public void Function()
        {
            Debug.Log("Function On");
        }
    }
}
