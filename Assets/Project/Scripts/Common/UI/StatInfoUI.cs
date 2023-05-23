using TMPro;
using UnityEngine;

namespace Common.UI
{
    public class StatInfoUI : MonoBehaviour, IEditable
    {
        [SerializeField] private StatType statType;
        [SerializeField] private TextMeshProUGUI labelText;
        [SerializeField] private TextMeshProUGUI valueText;

        public StatType StatType => statType;


        public void SetValue(string value)
        {
            valueText.text = value;
        }
        
        public void SetValue(Stat stat)
        {
            valueText.text = stat.Value.ToStatUIValue(statType);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            if (gameObject.name != statType.ToString()) 
                gameObject.name = statType.ToString();
            
            labelText      = transform.Find("Label").GetComponent<TextMeshProUGUI>();
            valueText      = transform.Find("Value").GetComponent<TextMeshProUGUI>();
            labelText.text = statType.ToString();
            valueText.text = "##.#";
        }
#endif
    }
}
