using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
    public class MaterialInfoUI : MonoBehaviour, IEditable
    {
        [SerializeField] private MaterialType materialType;
        [SerializeField] private Image materialImage;
        [SerializeField] private TextMeshProUGUI labelText;
        [SerializeField] private TextMeshProUGUI valueText;
        
        public MaterialType MaterialType => materialType;

        public void SetInfoUI(MaterialType type, string value)
        {
            materialType         = type;
            labelText.text       = materialType.ToString().DivideWords();
            valueText.text       = value;
            materialImage.sprite = materialType != MaterialType.None
                ? Database.MaterialSpriteData.Get((DataIndex)materialType) 
                : null;
        }
        
        public void SetValue(string value)
        {
            valueText.text = value;
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            materialImage        = transform.Find("Image").GetComponent<Image>();
            labelText            = transform.Find("Label").GetComponent<TextMeshProUGUI>();
            valueText            = transform.Find("Value").GetComponent<TextMeshProUGUI>();
            materialImage.sprite = materialType != MaterialType.None ? Database.MaterialSpriteData.Get((DataIndex)materialType) : null;
            labelText.text       = materialType.ToString().DivideWords();
            valueText.text       = "##.#";
            
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
