using System.Collections.Generic;
using Common;
using Common.Equipments;
using Common.PlayerCamps;
using UnityEngine;

namespace Lobby.UI
{
    public class InventoryUI : MonoBehaviour, IEditable
    {
        [SerializeField] private EquipType type;
        [SerializeField] private RectTransform header;
        [SerializeField] private RectTransform items;
        [SerializeField] private GameObject slotPrefab;

        private readonly List<ItemSlotUI> itemSlotList = new();
        
        
        public void AddInventorySlot(Equipment equipment)
        {
            var newSlot = Instantiate(slotPrefab, items);

            if (!newSlot.TryGetComponent(out ItemSlotUI itemSlotBehaviour))
            {
                Debug.LogError($"Not Exist {typeof(ItemSlotUI)} in {newSlot.name} Object");
                return;
            }
            
            itemSlotBehaviour.SetItemUI(equipment);
            itemSlotList.Add(itemSlotBehaviour);
            
            DynamicContentSizeFitting();
        }

        public void RemoveInventorySlot(Equipment equipment)
        {
            ItemSlotUI targetSlot = null;
            
            foreach (var itemSlot in itemSlotList)
            {
                if (itemSlot.Equipment != equipment) continue;
                
                targetSlot = itemSlot;
                break;
            }

            itemSlotList.Remove(targetSlot);
            Destroy(targetSlot);
            
            DynamicContentSizeFitting();
        }


        private void Awake()
        {
            var playerInventory = PlayerCamp.Inventories.GetInventoryByType(type);
            
            playerInventory.GetList().ForEach(AddInventorySlot);
            DynamicContentSizeFitting();
        }

        /* UI */
        public void DynamicContentSizeFitting()
        {
            var sizeUnit          = 110;
            var bottomMargin      = 10f;
            var rowCount          = itemSlotList.IsNullOrEmpty() ? 0 : itemSlotList.Count / 8 + 1;
            var subContentsHeight = header.sizeDelta.y + rowCount * sizeUnit;

            TryGetComponent<RectTransform>(out var rect);

            var newSizeDelta = new Vector2(rect.sizeDelta.x, subContentsHeight + bottomMargin);

            rect.sizeDelta = newSizeDelta;
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            header = transform.Find("Header").GetComponent<RectTransform>();
            items  = transform.Find("Contents").GetComponent<RectTransform>();
        }
#endif
    }
}
