using Singleton;
using UnityEngine;
using UnityEngine.Serialization;

namespace Manager
{
    using Input;
    
    public class MainManager : MonoSingleton<MainManager>
    {
        [FormerlySerializedAs("inputManager")] [SerializeField] private OldInputManager oldInputManager;

        public static OldInputManager oldInput => Instance ? Instance.oldInputManager : null;
    }
}