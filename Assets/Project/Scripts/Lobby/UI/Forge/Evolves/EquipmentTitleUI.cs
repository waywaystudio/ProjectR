using Common;
using Common.PartyCamps;
using TMPro;
using UnityEngine;

namespace Lobby.UI.Forge.Evolves
{
    public class EquipmentTitleUI : MonoBehaviour, IEditable
    {
        [SerializeField] private TextMeshProUGUI titleTextMesh;
        [SerializeField] private EquipmentSlotType slotType;

        public void OnReloadForge()
        {
            var adventurer = LobbyDirector.UI.Forge.FocusAdventurer;
            var targetCharacter = PartyCamp.Characters.GetData(adventurer);
            var targetEquipment = targetCharacter.GetEquipment(slotType);

            if (targetEquipment != null)
            {
                var equipmentName = $"{targetEquipment.ItemName}";
                titleTextMesh.text = equipmentName;
            }
            else
            {
                titleTextMesh.text = "UnEquip";
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
