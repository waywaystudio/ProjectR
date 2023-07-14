using System.Collections;
using Character.Venturers;
using Singleton;
using UnityEngine;

namespace Lobby
{
    using UI;
    
    public class LobbyDirector : MonoSingleton<LobbyDirector>, IEditable
    {
        [SerializeField] private LobbyCameraDirector cameraDirector;
        [SerializeField] private LobbyCastingDirector castingDirector;
        [SerializeField] private LobbyInputDirector inputDirector;
        [SerializeField] private LobbyUIDirector uiDirector;
        [SerializeField] private GameEventVenturer venturerFocusEvent;
        
        private VenturerBehaviour focusVenturer;

        public static LobbyCameraDirector Camera => Instance.cameraDirector;
        public static LobbyCastingDirector Casting => Instance.castingDirector;
        public static LobbyInputDirector Input => Instance.inputDirector;
        public static LobbyUIDirector UI => Instance.uiDirector;
        public static VenturerBehaviour FocusVenturer
        {
            get => Instance.focusVenturer;
            set
            {
                Instance.focusVenturer = value;
                Instance.venturerFocusEvent.Invoke(value);
            }
        }

        /*
         * Accessor
         */

        /* UI */
        public static SaveLoadUI SaveLoadUI => UI.SaveLoadUI;
        public static ForgeUI Forge => UI.Forge;
        public static WorldMapUI WorldMap => UI.WorldMap;
        public static void DeActivePanels() => UI.DeActivePanels();
        
        public void ToggleSaveLoadPanel() => uiDirector.ToggleSaveLoadPanel();
        public void ToggleForgePanel() => uiDirector.ToggleForgePanel();
        public void ToggleRefineryPanel() => uiDirector.ToggleRefineryPanel();
        public void ToggleWorldMapPanel() => uiDirector.ToggleWorldMapPanel();

        private IEnumerator Start()
        {
            yield return null; 
            
            castingDirector.Initialize();
            cameraDirector.Initialize();
            inputDirector.Initialize();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            cameraDirector  ??= GetComponentInChildren<LobbyCameraDirector>();
            castingDirector ??= GetComponentInChildren<LobbyCastingDirector>();
            inputDirector   ??= GetComponentInChildren<LobbyInputDirector>();
            uiDirector      ??= GetComponentInChildren<LobbyUIDirector>();
        }
#endif
    }
}
