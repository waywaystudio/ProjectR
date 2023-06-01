using Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.UI.Forge.Upgrades
{
    public class VirtueInfoUI : MonoBehaviour, IEditable
    {
        [SerializeField] private EthosType ethosType;
        [SerializeField] private Slider deficiencyProgress;
        [SerializeField] private Slider excessProgress;
        [SerializeField] private TextMeshProUGUI virtueLevelMesh;


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            deficiencyProgress = transform.Find("VirtueStatUI").Find("DeficiencySlider").GetComponent<Slider>();
            excessProgress     = transform.Find("VirtueStatUI").Find("ExcessSlider").GetComponent<Slider>();
            virtueLevelMesh    = transform.Find("VirtueStatUI").Find("LevelTextUI").GetComponent<TextMeshProUGUI>();
            
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
