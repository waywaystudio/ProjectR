using System.Collections.Generic;
using Common;
using Common.Camps;
using UnityEngine;

namespace Lobby.UI
{
    public class MaterialInventoryUI : MonoBehaviour, IEditable
    {
        [SerializeField] private DataIndex category;
        [SerializeField] private RectTransform header;
        [SerializeField] private RectTransform itemsHierarchy;
        [SerializeField] private GameObject slotPrefab;

        public DataIndex Category => category;
        
        private readonly List<ViceMaterialSlotUI> itemSlotList = new();


        public void Reload()
        {
            // var materialTable = Camp.Inventories.ViceMaterialTable;
            // var arrayIndex    = 0;
            //
            // materialTable.ForEach(material =>
            // {
            //     if (itemSlotList.Count > arrayIndex)
            //     {
            //         itemSlotList[arrayIndex].SetItemUI(material.Key, material.Value);
            //     }
            //     else
            //     {
            //         AddInventorySlot(material.Key, material.Value);
            //     }
            //     
            //     arrayIndex++;
            // });
            //
            // DynamicContentSizeFitting();
            //
            // if (itemSlotList.Count == 0) 
            //     gameObject.SetActive(false);
        }
        
        public void AddInventorySlot(ViceMaterialType material, int count)
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
        
            var newSlot = Instantiate(slotPrefab, itemsHierarchy);
        
            if (!newSlot.TryGetComponent(out ViceMaterialSlotUI itemSlotBehaviour))
            {
                Debug.LogError($"Not Exist {typeof(ViceMaterialSlotUI)} in {newSlot.name} Object");
                return;
            }

            itemSlotBehaviour.SetItemUI(material, count);
            itemSlotList.Add(itemSlotBehaviour);
            
            DynamicContentSizeFitting();
        }
        
        public void RemoveInventorySlot(ViceMaterialType material)
        {
            ViceMaterialSlotUI targetSlot = null;
            
            foreach (var itemSlot in itemSlotList)
            {
                if (itemSlot.MaterialType != material) continue;
                
                targetSlot = itemSlot;
                break;
            }
        
            itemSlotList.Remove(targetSlot);
            
            if (targetSlot != null) 
                Destroy(targetSlot.gameObject);
        
            DynamicContentSizeFitting();
            
            if (itemSlotList.Count == 0) 
                gameObject.SetActive(false);
        }


        private void Awake()
        {
            // var materialTable = Camp.Inventories.ViceMaterialTable;
            //
            // materialTable.ForEach(material => AddInventorySlot(material.Key, material.Value));
            //
            // DynamicContentSizeFitting();
            //
            // if (itemSlotList.Count == 0) 
            //     gameObject.SetActive(false);
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
            itemsHierarchy  = transform.Find("Contents").GetComponent<RectTransform>();
        }
#endif
    }
}
