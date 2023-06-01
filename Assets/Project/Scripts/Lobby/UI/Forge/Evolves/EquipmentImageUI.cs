using Common;
using Common.PartyCamps;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.UI.Forge.Evolves
{
    public class EquipmentImageUI : MonoBehaviour, IEditable
    {
        [SerializeField] private Image equipmentImage;
        [SerializeField] private EquipmentSlotType slotType;
        [SerializeField] private Sprite defaultImage;

        public void OnReloadForge()
        {
            var adventurer = LobbyDirector.UI.Forge.FocusAdventurer;
            var targetCharacter = PartyCamp.Characters.GetData(adventurer);
            var targetEquipment = targetCharacter.GetEquipment(slotType);

            if (targetEquipment.IsNullOrDestroyed())
            {
                equipmentImage.sprite = defaultImage;
                return;
            }
            
            var equipmentDataIndex = targetEquipment.DataIndex;
            var sprite             = Database.EquipmentSpriteData.Get(equipmentDataIndex);

            if (sprite != null)
            {
                equipmentImage.sprite = sprite;
            }
        }
        
#if UNITY_EDITOR
        public void EditorSetUp()
        {
            equipmentImage ??= GetComponent<Image>();
        }
#endif
    }
}
