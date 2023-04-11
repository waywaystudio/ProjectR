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

        public void SetEquipmentInfoUI(EquipmentInfo info)
        {
            if (info == null)
            {
                SetDefault();
                return;
            }

            var equipment = Database.EquipmentMaster.Get<Equipment>(info.ActionCode);
            
            image.sprite       = equipment.Icon;
            equipmentName.text = equipment.Title;
        }


        private void SetDefault()
        {
            image.sprite       = null;
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
