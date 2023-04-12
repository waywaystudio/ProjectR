using Common;
using Common.Equipments;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.UI
{
    public class EquipmentInfoUI : MonoBehaviour, IEditable
    {
        [SerializeField] private EquipSlotIndex equipSlot;
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI equipmentName;

        public EquipSlotIndex EquipSlot => equipSlot;

        public void SetEquipmentInfoUI(Equipment equipment)
        {
            if (equipment is null || equipment.Info.ActionCode == DataIndex.None)
            {
                SetDefault();
                return;
            }
            
            image.sprite       = equipment.Icon;
            image.color        = Color.white;
            equipmentName.text = equipment.Title;
        }


        private void SetDefault()
        {
            // image.sprite       = Empty Equipment Icon
            image.color        = new Color(1, 1, 1, 0.05f);
            equipmentName.text = "Empty";
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            image         = transform.Find("Icon").Find("Contents").GetComponent<Image>();
            equipmentName = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        }
#endif
    }
}
