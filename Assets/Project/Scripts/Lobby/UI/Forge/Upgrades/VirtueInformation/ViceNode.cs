using Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.UI.Forge.Upgrades.VirtueInformation
{
    public class ViceNode : MonoBehaviour, IEditable
    {
        [SerializeField] private EthosType viceType;
        [SerializeField] private int chargeLevel;
        [SerializeField] private Image circleImage;
        [SerializeField] private TextMeshProUGUI levelTextMesh;
        
        


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            circleImage   = transform.Find("CircleFront").GetComponent<Image>();
            levelTextMesh = GetComponentInChildren<TextMeshProUGUI>();
        }
#endif
    }
}
