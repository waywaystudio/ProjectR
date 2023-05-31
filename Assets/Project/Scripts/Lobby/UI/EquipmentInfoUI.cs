using Common;
using Common.Equipments;
using Common.UI.Tooltips;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lobby.UI
{
    public class EquipmentInfoUI : MonoBehaviour, IEquipmentTooltip, IPointerClickHandler, IEditable
    {
        [SerializeField] private EquipmentSlotType equipSlot;
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI equipmentName;
        [SerializeField] private Sprite defaultSprite;

        public EquipmentSlotType EquipSlot => equipSlot;
        
        // TODO. TEMP :: 추후에 이벤트 방식 or 시스템화를 통해서 Drawer Dependency 해제.
        private EquipmentTooltipDrawer tooltipDrawer;
        private EquipmentTooltipDrawer TooltipDrawer => tooltipDrawer ??= GetComponent<EquipmentTooltipDrawer>();
        public EquipmentEntity EquipmentEntity { get; set; }


        public void SetEquipmentInfoUI(EquipmentEntity equipEntity)
        {
            if (equipEntity == null)
            {
                SetDefault();
                return;
            }

            var equipmentSprite = Database.EquipmentSpriteData.Get(equipEntity.DataIndex);
            
            // if (equipmentSprite is null) return;
            EquipmentEntity = equipEntity;
            
            if (equipmentSprite is null || equipEntity.DataIndex == DataIndex.None)
            {
                SetDefault();
                return;
            }
            
            image.sprite       = equipEntity.Icon;
            image.color        = new Color(1, 1, 1, 1);
            equipmentName.text = equipEntity.ItemName;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // Right Click To Disarm
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                Debug.LogWarning("Disarm Function is now on working...");
                
                // var adventurerData = LobbyDirector.AdventurerFrame.CurrentAdventurerData;
                // var disarmed       = adventurerData.EquipmentTable[equipSlot];
                // PlayerCamp.Inventories.Add(EquipmentInfo.CreateEquipment(disarmed, null));
                // adventurerData.EquipmentTable[equipSlot] = null;
                // TooltipDrawer.Hide();
                // SetDefault();
                // LobbyDirector.AdventurerFrame.ReloadAdventurer(adventurerData.ClassType);
                // LobbyDirector.InventoryFrame.Reload(disarmed.EquipType);
            }
        }


        private void SetDefault()
        {
            // Equipment          = null;
            image.sprite       = defaultSprite;
            image.color        = new Color(1, 1, 1, 0.05f);
            equipmentName.text = "Empty";
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            image         = transform.Find("Icon").Find("Contents").GetComponent<Image>();
            equipmentName = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        }
#endif
    }
}
