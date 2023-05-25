using Common;
using GameEvents;
using UnityEngine;

namespace Lobby.UI.Forge
{
    public class ForgeUI : MonoBehaviour, IEditable
    {
        // AdventurerFrame
        [SerializeField] private GameEvent reloadForgeEvent;
        // InventoryFrame
        
        [SerializeField] private EvolveFrame evolveFrame; 
        
        private CombatClassType focusAdventurer = CombatClassType.Knight;
        private EquipSlotIndex focusSlot = EquipSlotIndex.Weapon;
        private RelicType focusRelic = RelicType.Vowed;
        
        
        public CombatClassType FocusAdventurer
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


        public void Initialize()
        {
            evolveFrame.Initialize();
        }

        
        

#if UNITY_EDITOR
        public void EditorSetUp()
        {
            evolveFrame ??= GetComponentInChildren<EvolveFrame>();
        }
#endif
    }
}
