using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
    public class GrowMaterialInfoUI : MonoBehaviour, IEditable
    {
        [SerializeField] private GrowMaterialType materialType;
        [SerializeField] private Image materialImage;
        [SerializeField] private TextMeshProUGUI labelText;
        [SerializeField] private TextMeshProUGUI valueText;
        

        public void ReloadInfoUI()
        {
            var growMaterialCount = Camp.GetGrowMaterialCount(materialType);

            valueText.text = growMaterialCount == 0 
                ? "-" 
                : growMaterialCount.ToString();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            materialImage        = transform.Find("Image").GetComponent<Image>();
            labelText            = transform.Find("Label").GetComponent<TextMeshProUGUI>();
            valueText            = transform.Find("Value").GetComponent<TextMeshProUGUI>();
            materialImage.sprite = materialType != GrowMaterialType.None ? Database.MaterialSpriteData.Get((DataIndex)materialType) : null;
            labelText.text       = materialType.ToString().ToDivideWords();
            valueText.text       = "##.#";
            
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
