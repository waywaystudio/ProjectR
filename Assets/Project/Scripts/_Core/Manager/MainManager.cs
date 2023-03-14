using Singleton;
using UnityEngine;

namespace Manager
{
    using Audio;
    using Input;
    using Save;
    using Scene;
    
    public class MainManager : MonoSingleton<MainManager>
    {
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private SceneManager sceneManager;
        [SerializeField] private InputManager inputManager;

        public static AudioManager Audio => Instance ? Instance.audioManager : null;
        public static SaveManager Save => Instance ? Instance.saveManager : null;
        public static SceneManager Scene => Instance ? Instance.sceneManager : null;
        public static InputManager Input => Instance ? Instance.inputManager : null;

        protected override void Awake()
        {
            base.Awake();
            
            Application.targetFrameRate = 60;
            saveManager.Initialize();
        }
    }
}