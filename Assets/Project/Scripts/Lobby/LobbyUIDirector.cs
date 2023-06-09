using UnityEngine;

namespace Lobby
{
    using UI;
    
    public class LobbyUIDirector : MonoBehaviour, IEditable
    {
        [SerializeField] private GameObject serializePanel;
        [SerializeField] private GameObject forgePanel;
        [SerializeField] private GameObject refineryPanel;
        [SerializeField] private GameObject worldMapPanel;
        [SerializeField] private SaveLoadUI saveLoad;
        [SerializeField] private ForgeUI forge;
        [SerializeField] private WorldMapUI worldMap;

        public SaveLoadUI SaveLoadUI => saveLoad;
        public ForgeUI Forge => forge;
        // public RefineryUI
        public WorldMapUI WorldMap => worldMap;


        public void DeActivePanels()
        {
            serializePanel.SetActive(false);
            forgePanel.SetActive(false);
        }

        public void ToggleSaveLoadPanel() => TogglePanel(serializePanel);
        public void ToggleForgePanel() => TogglePanel(forgePanel);
        public void ToggleRefineryPanel() => TogglePanel(refineryPanel);
        public void ToggleWorldMapPanel() => TogglePanel(worldMapPanel);
        
        public static void TogglePanel(GameObject panel) { panel.SetActive(!panel.activeSelf); }

#if UNITY_EDITOR
        public void EditorSetUp()
        {
            serializePanel = transform.Find("Save&LoadManager").gameObject;
            forgePanel     = transform.Find("Forge").gameObject;
            refineryPanel  = transform.Find("Refinery").gameObject;
            worldMapPanel  = transform.Find("WorldMap").gameObject;

            saveLoad ??= GetComponentInChildren<SaveLoadUI>();
            forge    ??= GetComponentInChildren<ForgeUI>();
            worldMap ??= GetComponentInChildren<WorldMapUI>();
        }
#endif
    }
}
