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
        [SerializeField] private InventoryFrame inventoryFrame;
        [SerializeField] private BossSelectFrame bossSelectFrame;

        public static SaveLoadFrame SaveLoadFrame => Instance.saveLoadFrame;
        public static AdventurerFrame AdventurerFrame => Instance.adventurerFrame;
        public static InventoryFrame InventoryFrame => Instance.inventoryFrame;
        public static BossSelectFrame BossSelectFrame => Instance.bossSelectFrame;


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            saveLoadFrame   ??= GetComponentInChildren<SaveLoadFrame>();
            adventurerFrame ??= GetComponentInChildren<AdventurerFrame>();
            inventoryFrame  ??= GetComponentInChildren<InventoryFrame>();
        }
#endif
    }
}
