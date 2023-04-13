using Common;
using Common.Equipments;
using Common.PlayerCamps;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lobby.UI
{
    public class EquipmentInfoUI : MonoBehaviour, IPointerClickHandler, IEditable
    {
        [SerializeField] private EquipSlotIndex equipSlot;
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI equipmentName;
        [SerializeField] private Sprite defaultSprite;

        public EquipSlotIndex EquipSlot => equipSlot;

        public void SetEquipmentInfoUI(EquipmentInfo equipInfo)
        {
            if (!Database.EquipmentMaster.Get(equipInfo.ActionCode, out Equipment equipment)) return;
            
            if (equipment is null || equipInfo.ActionCode == DataIndex.None)
            {
                SetDefault();
                return;
            }
            
            image.sprite       = equipment.Icon;
            image.color        = new Color(1, 1, 1, 1);
            equipmentName.text = equipment.Title;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // Right Click To Equip
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                var adventurer = LobbyDirector.AdventurerFrame.CurrentCharacter;

                if (!adventurer.Equipment.TryDisarm(equipSlot, out var disarmed)) return;
                
                PlayerCamp.Inventories.Add(disarmed);
                SetDefault();
                
                LobbyDirector.AdventurerFrame.ReloadAdventurer(adventurer.CombatClass);
                LobbyDirector.InventoryFrame.Reload(disarmed.EquipType);
            }
        }


        private void SetDefault()
        {
            // TODO.Empty Equipment Icon
            image.sprite       = defaultSprite;
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
