using Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.UI
{
    public class ViceMaterialSlotUI : MonoBehaviour, IEditable
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TextMeshProUGUI itemCountText;

        public ViceMaterialType MaterialType { get; set; }


        public void SetItemUI(ViceMaterialType type, int count)
        {
            MaterialType       = type;
            itemCountText.text = count.ToString();
            itemImage.sprite   = Database.MaterialSpriteData.Get((DataIndex)type);
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
