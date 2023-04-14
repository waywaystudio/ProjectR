using System;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Lobby.UI
{
    public class InventoryFrame : MonoBehaviour, IEditable
    {
        [SerializeField] private List<InventoryUI> inventoryUIList;


        public void ReloadAll()
        {
            inventoryUIList.ForEach(inventory => inventory.Reload());
        }

        public void Reload(EquipType slotType) => GetInventoryUI(slotType).Reload();

        public InventoryUI GetInventoryUI(EquipType slotType)
            => inventoryUIList.TryGetElement(inventoryUI => inventoryUI.EquipType == slotType);


        private void Awake()
        {
            // ReloadAll();
        }

#if UNITY_EDITOR
        public void EditorSetUp()
        {
            GetComponentsInChildren(inventoryUIList);
        }
#endif
    }
}
