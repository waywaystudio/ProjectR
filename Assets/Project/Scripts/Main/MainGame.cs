using Main.Audio;
using Main.Input;
using Main.Save;
using UnityEngine;
using Wayway.Engine.Singleton;

namespace Main
{
    public class MainGame : MonoSingleton<MainGame>
    {
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private InputManager inputManager;

        public static AudioManager AudioManager => Instance.audioManager;
        public static SaveManager SaveManager => Instance.saveManager;
        public static InputManager InputManager => Instance.inputManager;

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
