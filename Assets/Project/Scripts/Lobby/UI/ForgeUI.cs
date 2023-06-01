using System;
using Common;
using Common.Characters;
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

        public VenturerType FocusAdventurer
        {
            get => focusAdventurer;
            set
            {
                focusAdventurer = value;
                reloadForgeEvent.Invoke();
            }
        }

        public CharacterData FocusVenturerData => PartyCamp.Characters.GetData(FocusAdventurer);

        public EquipmentEntity VenturerEquipment(EquipmentSlotType slot)
        {
            return PartyCamp.Characters.GetAdventurerEquipment(FocusAdventurer, slot);
        }

        public int VenturerEthosValue(EthosType type) => FocusVenturerData.GetEthosValue(type);

        public void Conversion()
        {
            // => FocusEquipment.Conversion(FocusRelic);
        }

        public void Enchant()
        {
            // => FocusEquipment.Enchant(FocusRelic);
        }
        


#if UNITY_EDITOR
        private void Awake()
        {
            reloadForgeEvent.Invoke();
        }

        public void EditorSetUp()
        {
            // evolveFrame ??= GetComponentInChildren<EvolveFrame>();
        }

        private void Test()
        {
            reloadForgeEvent.Invoke();
        }
#endif
    }
}
