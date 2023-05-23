using UnityEngine;

namespace Lobby
{
    public class LobbyUIDirector : MonoBehaviour, IEditable
    {
        [SerializeField] private GameObject serializePanel;
        [SerializeField] private GameObject forgePanel;

        public void DeActivePanels()
        {
            serializePanel.SetActive(false);
            forgePanel.SetActive(false);
        }

        public void ToggleSerializePanel() => TogglePanel(serializePanel);
        public void ToggleForgePanel() => TogglePanel(forgePanel);
        
        public void TogglePanel(GameObject panel) { panel.SetActive(!panel.activeSelf); }

#if UNITY_EDITOR
        public void EditorSetUp()
        {
            serializePanel = transform.Find("Serialize").gameObject;
            forgePanel     = transform.Find("Forge").gameObject;
        }
#endif
    }
}
