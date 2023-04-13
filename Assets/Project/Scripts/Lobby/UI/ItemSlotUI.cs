using Common.Equipments;
using Common.PlayerCamps;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lobby.UI
{
    public class ItemSlotUI : MonoBehaviour, IPointerClickHandler, IEditable
    {
        [SerializeField] private Image itemImage;

        [Sirenix.OdinInspector.ShowInInspector]
        public Equipment Equipment { get; private set; }

        private InventoryUI MasterInventoryUI { get; set; }


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
