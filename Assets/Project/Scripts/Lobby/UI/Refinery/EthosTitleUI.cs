using Common;
using TMPro;
using UnityEngine;

namespace Lobby.UI.Refinery
{
    public class EthosTitleUI : MonoBehaviour, IEditable
    {
        [SerializeField] private EthosType virtueType;
        [SerializeField] private TextMeshProUGUI deficiencyPointTextMesh;
        [SerializeField] private TextMeshProUGUI excessPointTextMesh;
        
        private EthosType DeficiencyType => virtueType.GetSameThemeDeficiency();
        private EthosType ExcessType => virtueType.GetSameThemeExcess();


        public void OnEthosChanged()
        {
            if (!virtueType.IsVirtue()) return;

            var deficiencyValue = LobbyDirector.Forge.VenturerEthosValue(DeficiencyType);
            var excessValue = LobbyDirector.Forge.VenturerEthosValue(ExcessType);

            deficiencyPointTextMesh.text = deficiencyValue.ToRoman();
            excessPointTextMesh.text     = excessValue.ToRoman();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            var virtueInfoUI = GetComponentInParent<VirtueInfoUI>();
            
            deficiencyPointTextMesh = transform.Find("Deficiency PointTextUI").GetComponent<TextMeshProUGUI>();
            excessPointTextMesh     = transform.Find("Excess PointTextUI").GetComponent<TextMeshProUGUI>();
            virtueType              = virtueInfoUI.VirtueType;
            
            if (!virtueType.IsVirtue())
            {
                Debug.LogWarning($"EthosTitleUI GameObject's virtueType value missing. Input:{virtueType}");
                return;
            }

            var meanTitleTextMesh       = transform.Find("MeanTitle").GetComponent<TextMeshProUGUI>();
            var deficiencyTitleTextMesh = transform.Find("DeficiencyTitle").GetComponent<TextMeshProUGUI>();
            var excessTitleTextMesh     = transform.Find("ExcessTitle").GetComponent<TextMeshProUGUI>();
            var deficiencyValue = LobbyDirector.Forge.VenturerEthosValue(DeficiencyType);
            var excessValue = LobbyDirector.Forge.VenturerEthosValue(ExcessType);

            meanTitleTextMesh.text       = virtueType.ToString();
            deficiencyTitleTextMesh.text = DeficiencyType.ToString();
            excessTitleTextMesh.text     = ExcessType.ToString();
            
            deficiencyPointTextMesh.text = deficiencyValue.ToRoman();
            excessPointTextMesh.text     = excessValue.ToRoman();
        }
#endif
    }
}
