using Common.PartyCamps;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.UI.Forge.Evolves
{
    public class EquipmentImageUI : MonoBehaviour, IEditable
    {
        [SerializeField] private Image equipmentImage;
        [SerializeField] private Sprite defaultImage;

        public void OnReloadForge()
        {
            var adventurer = LobbyDirector.UI.Forge.FocusAdventurer;
            var slot       = LobbyDirector.UI.Forge.FocusSlot;
            
            // TODO. Relic 별로 이미지가 다르다면 참조.
            // var relic      = LobbyDirector.UI.Forge.FocusRelic;

            var targetCharacter    = PartyCamp.Characters.GetData(adventurer);
            var targetEquipment    = targetCharacter.GetEquipment(slot);

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
