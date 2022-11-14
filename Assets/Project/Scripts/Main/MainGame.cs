using Core.Singleton;
using Main.Manager.Audio;
using Main.Manager.Input;
using Main.Manager.Save;
using Main.Manager.Scene;
using Main.UI;
using UnityEngine;

namespace Main
{
    public class MainGame : MonoSingleton<MainGame>
    {
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private InputManager inputManager;
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private SceneManager sceneManager;
        [SerializeField] private MainUI ui;

        public static AudioManager AudioManager => Instance.audioManager;
        public static InputManager InputManager => Instance.inputManager;
        public static SaveManager SaveManager => Instance.saveManager;
        public static SceneManager SceneManager => Instance.sceneManager;
        public static MainUI UI => Instance.ui;

        protected override void Awake()
        {
            base.Awake();
            
            saveManager.Initialize();
        }

        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        [Sirenix.OdinInspector.Button]
        public void Function()
        {
            Debug.Log("Function On");
        }
    }
}
