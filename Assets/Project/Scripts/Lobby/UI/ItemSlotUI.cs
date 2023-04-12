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

        public Equipment Equipment { get; private set; }


        public void SetItemUI(Equipment equipment)
        {
            Equipment = equipment;

            itemImage.sprite = equipment.Icon;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                var adventurer = LobbyDirector.AdventurerFrame.CurrentCharacter;
                
                adventurer.Equipment.Equip(Equipment, out var disarmed);

                PlayerCamp.Inventories.Remove(Equipment);
                PlayerCamp.Inventories.Add(disarmed);
                
                SetItemUI(disarmed);
                
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
