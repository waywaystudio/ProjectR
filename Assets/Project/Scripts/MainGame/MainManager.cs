using Core.Singleton;
using UnityEngine;

namespace MainGame
{
    using Manager.Audio;
    using Manager.Save;
    using Manager.Scene;
    using Manager.Input;
    
    public class MainManager : MonoSingleton<MainManager>
    {
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private SceneManager sceneManager;
        [SerializeField] private InputManager inputManager;

        public static AudioManager Audio => Instance.audioManager;
        public static SaveManager Save => Instance.saveManager;
        public static SceneManager Scene => Instance.sceneManager;
        public static InputManager Input => Instance.inputManager;

        protected override void Awake()
        {
            base.Awake();
            
            Application.targetFrameRate = 60;
            saveManager.Initialize();
        }
    }
}
