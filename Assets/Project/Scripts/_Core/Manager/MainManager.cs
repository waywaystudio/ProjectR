using Singleton;
using UnityEngine;

namespace Manager
{
    using Audio;
    using Input;
    
    public class MainManager : MonoSingleton<MainManager>
    {
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private InputManager inputManager;

        public static AudioManager Audio => Instance ? Instance.audioManager : null;
        public static InputManager Input => Instance ? Instance.inputManager : null;
    }
}