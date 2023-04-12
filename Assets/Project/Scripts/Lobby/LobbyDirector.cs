using Lobby.UI;
using Singleton;
using UnityEngine;

namespace Lobby
{
    public class LobbyDirector : MonoSingleton<LobbyDirector>, IEditable
    {
        [SerializeField] private LobbyCameraDirector cameraDirector;

        // TODO. will be UIDirector
        [SerializeField] private SaveLoadFrame saveLoadFrame;
        [SerializeField] private AdventurerFrame adventurerFrame;

        public static SaveLoadFrame SaveLoadFrame => Instance.saveLoadFrame;
        public static AdventurerFrame AdventurerFrame => Instance.adventurerFrame;


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            saveLoadFrame   ??= GetComponentInChildren<SaveLoadFrame>();
            adventurerFrame ??= GetComponentInChildren<AdventurerFrame>();
        }
#endif
    }
}
