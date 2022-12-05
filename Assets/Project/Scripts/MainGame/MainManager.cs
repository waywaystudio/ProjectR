using Core.Singleton;
using UnityEngine;

namespace MainGame
{
    using Manager.Audio;
    using Manager.Combat;
    using Manager.Input;
    using Manager.Save;
    using Manager.Scene;
    
    public class MainManager : MonoSingleton<MainManager>
    {
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private CombatManager combatManager;
        [SerializeField] private InputManager inputManager;
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private SceneManager sceneManager;

        public static AudioManager Audio => Instance.audioManager;
        public static CombatManager Combat => Instance.combatManager;
        public static InputManager InputManager => Instance.inputManager;
        public static SaveManager Save => Instance.saveManager;
        public static SceneManager SceneManager => Instance.sceneManager;

        protected override void Awake()
        {
            base.Awake();
            
            Application.targetFrameRate = 60;
            saveManager.Initialize();
        }
    }
}
