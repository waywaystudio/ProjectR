using Common;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.UI.Refinery
{
    public class ViceNode : MonoBehaviour, IEditable
    {
        [SerializeField] private EthosType viceType;
        [SerializeField] private int chargeLevel;
        [SerializeField] private Color onColor = Color.red;
        [SerializeField] private Image circleImage;

        public int ChargeLevel { get => chargeLevel; set => chargeLevel = value; }
        

        public void OnNode()
        {
            circleImage.color = onColor;
        }

        public void OffNode()
        {
            circleImage.color = Color.white;
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            circleImage   = transform.Find("CircleFront").GetComponent<Image>();
        }

        public void EditorViceNodeSetUp(EthosType viceType) => this.viceType = viceType;
#endif
    }
}
