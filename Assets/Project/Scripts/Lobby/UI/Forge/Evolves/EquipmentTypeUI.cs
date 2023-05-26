using Common;
using TMPro;
using UnityEngine;

namespace Lobby.UI.Forge.Evolves
{
    public class EquipmentTypeUI : MonoBehaviour, IEditable
    {
        [SerializeField] private TextMeshProUGUI equipmentTypeText;

        private EquipSlotIndex CurrentSlot { get; set; } = EquipSlotIndex.None;

        
        public void OnReloadForge()
        {
            if (CurrentSlot == LobbyDirector.UI.Forge.FocusSlot) return;

            CurrentSlot            = LobbyDirector.UI.Forge.FocusSlot;
            equipmentTypeText.text = CurrentSlot.ToString();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            equipmentTypeText ??= GetComponent<TextMeshProUGUI>();
        }
#endif
    }
}
