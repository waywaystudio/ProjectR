using Common;
using TMPro;
using UnityEngine;

namespace Lobby.UI.WorldMaps
{
    public class VillainNameUI : MonoBehaviour, IEditable
    {
        [SerializeField] private TextMeshProUGUI titleTextMesh;
        [SerializeField] private TextMeshProUGUI subTitleTextMesh;

        public void OnFocusVillainUIChanged()
        {
            if (LobbyDirector.WorldMap.FocusVillain == VillainType.None) return;
            
            var villain = LobbyDirector.WorldMap.FocusVillain;
            var data = Den.GetVillainData(villain);

            titleTextMesh.text    = data.FullName;
            subTitleTextMesh.text = data.SubName;
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            titleTextMesh    = transform.Find("Title").GetComponent<TextMeshProUGUI>();
            subTitleTextMesh = transform.Find("SubTitle").GetComponent<TextMeshProUGUI>();
        }
#endif
    }
}
