using Common;
using TMPro;
using UnityEngine;

namespace Lobby.UI.Forge.Upgrades.VirtueInformation
{
    public class VirtueLevelTextUI : MonoBehaviour, IEditable
    {
        [SerializeField] private EthosType ethosType;
        [SerializeField] private TextMeshProUGUI levelTextMesh;
        
        
        public void OnEthosChanged()
        {
            var virtueLevel = LobbyDirector.UI.Forge.VenturerEthosValue(ethosType);
            var levelText = virtueLevel == 0 
                ? "" 
                : virtueLevel.ToRoman();

            levelTextMesh.text = levelText;
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            var virtueInfoUI = GetComponentInParent<VirtueInfoUI>();
            
            levelTextMesh = GetComponent<TextMeshProUGUI>();
            ethosType     = virtueInfoUI.VirtueType;
            
            OnEthosChanged();
        }
#endif
    }
}
