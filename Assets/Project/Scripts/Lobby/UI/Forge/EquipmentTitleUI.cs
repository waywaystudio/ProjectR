using Common;
using TMPro;
using UnityEngine;

namespace Lobby.UI.Forge
{
    public class EquipmentTitleUI : MonoBehaviour, IEditable
    {
        [SerializeField] private TextMeshProUGUI titleTextMesh;
        [SerializeField] private EquipmentSlotType slotType;

        public void OnReloadForge()
        {
            var targetEquipment = slotType == EquipmentSlotType.Weapon
                ? LobbyDirector.Forge.VenturerWeapon()
                : LobbyDirector.Forge.VenturerArmor();

            if (targetEquipment != null)
            {
                var equipmentName = $"{targetEquipment.ItemName} {targetEquipment.Level}";
                titleTextMesh.text = equipmentName;
            }
            else
            {
                titleTextMesh.text = "UnEquipped";
            }
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            titleTextMesh ??= GetComponent<TextMeshProUGUI>();
        }
#endif
    }
}
