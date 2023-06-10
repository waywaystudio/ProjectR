// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;
//
// namespace Common.UI
// {
//     public class ViceMaterialInfoUI : MonoBehaviour, IEditable
//     {
//         [SerializeField] private ViceMaterialType materialType;
//         [SerializeField] private Image materialImage;
//         [SerializeField] private TextMeshProUGUI labelText;
//         [SerializeField] private TextMeshProUGUI valueText;
//         
//         public ViceMaterialType ViceMaterialType => materialType;
//
//
//         public void SetInfoUI(ViceIngredient viceIngredient) => SetInfoUI(viceIngredient.Type, viceIngredient.Count.ToString());
//         public void SetInfoUI(ViceMaterialType type, string value)
//         {
//             materialType         = type;
//             labelText.text       = materialType.ToString().DivideWords();
//             valueText.text       = value;
//             materialImage.sprite = materialType != ViceMaterialType.None
//                 ? Database.MaterialSpriteData.Get((DataIndex)materialType) 
//                 : null;
//         }
//         
//         public void SetValue(string value)
//         {
//             valueText.text = value;
//         }
//
//
// #if UNITY_EDITOR
//         public void EditorSetUp()
//         {
//             materialImage        = transform.Find("Image").GetComponent<Image>();
//             labelText            = transform.Find("Label").GetComponent<TextMeshProUGUI>();
//             valueText            = transform.Find("Value").GetComponent<TextMeshProUGUI>();
//             materialImage.sprite = materialType != ViceMaterialType.None ? Database.MaterialSpriteData.Get((DataIndex)materialType) : null;
//             labelText.text       = materialType.ToString().DivideWords();
//             valueText.text       = "##.#";
//             
//             UnityEditor.EditorUtility.SetDirty(this);
//         }
// #endif
//     }
// }
