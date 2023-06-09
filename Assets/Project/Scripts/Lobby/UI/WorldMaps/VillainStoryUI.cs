using Common;
using TMPro;
using UnityEngine;

namespace Lobby.UI.WorldMaps
{
    public class VillainStoryUI : MonoBehaviour, IEditable
    {
        [SerializeField] private TextMeshProUGUI storyTextMesh;
        
        public void OnFocusVillainUIChanged()
        {
            // Not targeted
            if (LobbyDirector.WorldMap.FocusVillain != VillainType.None)
            {
                var villain = LobbyDirector.WorldMap.FocusVillain;
                var data = Den.GetVillainData(villain);

                storyTextMesh.text    = data.Description;
            }
        }

        public void EditorSetUp()
        {
            storyTextMesh = transform.Find("Viewport").Find("Content").GetComponent<TextMeshProUGUI>();
        }
    }
}
