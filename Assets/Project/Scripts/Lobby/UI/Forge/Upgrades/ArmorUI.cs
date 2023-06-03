using Common;
using Common.Equipments;
using UnityEngine;

namespace Lobby.UI.Forge.Upgrades
{
    public class ArmorUI : MonoBehaviour
    {
        [SerializeField] private EquipmentSlotType slotType;

        private EquipmentEntity Equipment => LobbyDirector.UI.Forge.VenturerEquipment(slotType);
        
        public void Upgrade()
        {
            Equipment.Upgrade();
            LobbyDirector.UI.Forge.Reload();
        }
    }
}
