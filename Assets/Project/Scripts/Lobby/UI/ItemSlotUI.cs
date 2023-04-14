using Common;
using Common.Equipments;
using Common.PlayerCamps;
using Common.UI.Tooltips;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lobby.UI
{
    public class ItemSlotUI : MonoBehaviour, IEquipmentTooltip, IPointerClickHandler, IEditable
    {
        [SerializeField] private Image itemImage;

        [Sirenix.OdinInspector.ShowInInspector]
        public Equipment Equipment { get; private set; }

        private InventoryUI MasterInventoryUI { get; set; }
        
        // TODO. TEMP :: 추후에 이벤트 방식 or 시스템화를 통해서 Drawer Dependency 해제.
        private EquipmentTooltipDrawer tooltipDrawer;
        private EquipmentTooltipDrawer TooltipDrawer => tooltipDrawer ??= GetComponent<EquipmentTooltipDrawer>();


        public void SetItemUI(InventoryUI master, Equipment equipment)
        {
            MasterInventoryUI = master;
            Equipment         = equipment;
            itemImage.sprite  = equipment.Icon;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            // Right Click To Equip
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                var adventurer = LobbyDirector.AdventurerFrame.CurrentCharacter;
                
                adventurer.Equipment.Equip(Equipment, out var disarmed);

                PlayerCamp.Inventories.Remove(Equipment);

                // Replace
                if (disarmed != null)
                {
                    PlayerCamp.Inventories.Add(disarmed);
                
                    SetItemUI(MasterInventoryUI, disarmed);
                    TooltipDrawer.Draw();
                }
                // Equip Character at Empty Slot
                else
                {
                    MasterInventoryUI.RemoveInventorySlot(Equipment);
                }

                LobbyDirector.AdventurerFrame.ReloadAdventurer(adventurer.CombatClass);
            }
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            itemImage = transform.Find("Contents").GetComponent<Image>();
        }
#endif
    }
}
