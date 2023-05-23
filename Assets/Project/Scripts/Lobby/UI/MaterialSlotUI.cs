using Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.UI
{
    public class MaterialSlotUI : MonoBehaviour, IEditable
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TextMeshProUGUI itemCountText;

        public MaterialType MaterialType { get; set; }


        public void SetItemUI(MaterialType type, int count)
        {
            MaterialType       = type;
            itemCountText.text = count.ToString();
            itemImage.sprite   = Database.MaterialSpriteData.Get(type.ConvertToDataIndex());
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            itemImage     = transform.Find("Contents").GetComponent<Image>();
            itemCountText = transform.Find("Count").GetComponent<TextMeshProUGUI>();
        }
#endif
    }
}
