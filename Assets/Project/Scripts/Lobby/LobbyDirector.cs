using Lobby.UI;
using Singleton;
using UnityEngine;

namespace Lobby
{
    public class LobbyDirector : MonoSingleton<LobbyDirector>
    {
        [SerializeField] private LobbyCameraDirector cameraDirector;
        
        // TODO. will be UIDirector
        [SerializeField] private SaveLoadFrame saveLoadFrame;

        public static SaveLoadFrame SaveLoadFrame => Instance.saveLoadFrame;
        

        // protected override void Awake()
        // {
        //     base.Awake();
        //     
        //     cameraDirector ??= GetComponentInChildren<LobbyCameraDirector>();
        // }
    }
}
