using Character.Adventurers;
using Common;
using Lobby.UI.Forge;
using Singleton;
using UnityEngine;
using UnityEngine.Serialization;

namespace Lobby
{
    using UI;
    
    public class LobbyDirector : MonoSingleton<LobbyDirector>, IEditable
    {
        [SerializeField] private LobbyCameraDirector cameraDirector;
        [SerializeField] private LobbyCastingDirector castingDirector;
        [SerializeField] private LobbyInputDirector inputDirector;
        [SerializeField] private LobbyUIDirector uiDirector;

        public static LobbyCameraDirector Camera => Instance.cameraDirector;
        public static LobbyCastingDirector Casting => Instance.castingDirector;
        public static LobbyInputDirector Input => Instance.inputDirector;
        public static LobbyUIDirector UI => Instance.uiDirector;

        /*
         * Accessor
         */
        /* Casting */
        public static Adventurer Knight { get; set; }

        /* UI */
        public ForgeUI ForgeUI => UI.Forge;
              

        // TODO. will be UIDirector
        [SerializeField] private SaveLoadFrame saveLoadFrame;
        [FormerlySerializedAs("adventurerFrame")] [SerializeField] private OldAdventurerFrame oldAdventurerFrame;
        [SerializeField] private InventoryFrame inventoryFrame;
        [SerializeField] private BossSelectFrame bossSelectFrame;

        public static SaveLoadFrame SaveLoadFrame => Instance.saveLoadFrame;
        public static OldAdventurerFrame OldAdventurerFrame => Instance.oldAdventurerFrame;
        public static InventoryFrame InventoryFrame => Instance.inventoryFrame;
        public static BossSelectFrame BossSelectFrame => Instance.bossSelectFrame;

        protected override void Awake()
        {
            castingDirector.Initialize(); // Casting Director First
            cameraDirector.Initialize(Knight.transform);
            inputDirector.Initialize(Knight);
            uiDirector.Initialize();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            cameraDirector  ??= GetComponentInChildren<LobbyCameraDirector>();
            castingDirector ??= GetComponentInChildren<LobbyCastingDirector>();
            inputDirector   ??= GetComponentInChildren<LobbyInputDirector>();
            uiDirector      ??= GetComponentInChildren<LobbyUIDirector>();
            
            saveLoadFrame   ??= GetComponentInChildren<SaveLoadFrame>();
            oldAdventurerFrame ??= GetComponentInChildren<OldAdventurerFrame>();
            inventoryFrame  ??= GetComponentInChildren<InventoryFrame>();
        }
#endif
    }
}
