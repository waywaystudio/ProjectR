using TMPro;
using UnityEngine;

namespace Common.UI
{
    public class ViceInfoUI : MonoBehaviour, IEditable
    {
        [SerializeField] private EthosType type;
        [SerializeField] private TextMeshProUGUI labelText;
        [SerializeField] private TextMeshProUGUI valueText;


        public void SetVice(EthosType type, int value)
        {
            this.type      = type;
            labelText.text = type.ToString();
            valueText.text = value.ToString();
        }

        public void SetVice(EthosEntity ethos)
        {
            type           = ethos.EthosType;
            labelText.text = type.ToString();
            valueText.text = ethos.Value.ToString();
        }
        
        
        
#if UNITY_EDITOR
        public void EditorSetUp()
        {
            labelText      = transform.Find("Label").GetComponent<TextMeshProUGUI>();
            valueText      = transform.Find("Value").GetComponent<TextMeshProUGUI>();
            labelText.text = type.ToString();
            valueText.text = "##.#";
        }
#endif
    }
}
