using Core.Singleton;
using Main.Audio;
using Main.Input;
using Main.Save;
using Main.Scene;
using Main.UI;
using UnityEngine;
using Debug = Debug;

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
        
        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        [Sirenix.OdinInspector.Button]
        public void Function()
        {
            global::Debug.Log("Function On");
        }
    }
}
