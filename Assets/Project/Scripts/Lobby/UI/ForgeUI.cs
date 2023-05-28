using Common;
using Common.Equipments;
using Common.PartyCamps;
using GameEvents;
using UnityEngine;

namespace Lobby.UI
{
    public class ForgeUI : MonoBehaviour, IEditable
    {
        [SerializeField] private GameEvent reloadForgeEvent;

        // [SerializeField] private AdventurerFrame adventurerFrame;
        // [SerializeField] private EvolveFrame evolveFrame; 
        // InventoryFrame
        
        private VenturerType focusAdventurer = VenturerType.Knight;
        private EquipSlotIndex focusSlot = EquipSlotIndex.Weapon;
        private RelicType focusRelic = RelicType.VowedRelic;

        public VenturerType FocusAdventurer
        {
            get => focusAdventurer;
            set
            {
                focusAdventurer = value;
                reloadForgeEvent.Invoke();
            }
        }
        public EquipSlotIndex FocusSlot
        {
            get => focusSlot;
            set
            {
                focusSlot = value;
                reloadForgeEvent.Invoke();
            }
        }
        public RelicType FocusRelic
        {
            get => focusRelic;
            set
            {
                focusRelic = value;
                reloadForgeEvent.Invoke();
            }
        }

        public EquipmentEntity FocusEquipment => PartyCamp.Characters.GetAdventurerEquipment(FocusAdventurer, FocusSlot);
        
        public void NextEquipmentSlot() => FocusSlot = FocusSlot.NextExceptNone();
        public void PrevEquipmentSlot() => FocusSlot = FocusSlot.PrevExceptNone();
        public void Conversion() => FocusEquipment.Conversion(FocusRelic);
        public void Enchant() => FocusEquipment.Enchant(FocusRelic);
        


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            // evolveFrame ??= GetComponentInChildren<EvolveFrame>();
        }
#endif
    }
}
