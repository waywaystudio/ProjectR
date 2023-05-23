using Character.Adventurers;
using Lobby.UI;
using Singleton;
using UnityEngine;

namespace Lobby
{
    public class LobbyDirector : MonoSingleton<LobbyDirector>, IEditable
    {
        [SerializeField] private LobbyCameraDirector cameraDirector;
        [SerializeField] private LobbyCastingDirector castingDirector;
        [SerializeField] private LobbyInputDirector inputDirector;
        [SerializeField] private LobbyUIDirector uiDirector;

        public static LobbyCameraDirector CameraDirector => Instance.cameraDirector;
        public static LobbyCastingDirector CastingDirector => Instance.castingDirector;
        public static LobbyInputDirector InputDirector => Instance.inputDirector;
        public static LobbyUIDirector UIDirector => Instance.uiDirector;

        public static Adventurer Knight { get; set; }

        // TODO. will be UIDirector
        [SerializeField] private SaveLoadFrame saveLoadFrame;
        [SerializeField] private AdventurerFrame adventurerFrame;
        [SerializeField] private InventoryFrame inventoryFrame;
        [SerializeField] private BossSelectFrame bossSelectFrame;

        public static SaveLoadFrame SaveLoadFrame => Instance.saveLoadFrame;
        public static AdventurerFrame AdventurerFrame => Instance.adventurerFrame;
        public static InventoryFrame InventoryFrame => Instance.inventoryFrame;
        public static BossSelectFrame BossSelectFrame => Instance.bossSelectFrame;

        protected override void Awake()
        {
            castingDirector.Initialize();
            cameraDirector.Initialize(Knight.transform);
            inputDirector.Initialize(Knight);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            cameraDirector  ??= GetComponentInChildren<LobbyCameraDirector>();
            castingDirector ??= GetComponentInChildren<LobbyCastingDirector>();
            inputDirector   ??= GetComponentInChildren<LobbyInputDirector>();
            uiDirector      ??= GetComponentInChildren<LobbyUIDirector>();
            
            saveLoadFrame   ??= GetComponentInChildren<SaveLoadFrame>();
            adventurerFrame ??= GetComponentInChildren<AdventurerFrame>();
            inventoryFrame  ??= GetComponentInChildren<InventoryFrame>();
        }
#endif
    }
}
