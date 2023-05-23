using System.Collections.Generic;
using UnityEngine;

namespace Lobby.UI
{
    public class InventoryFrame : MonoBehaviour, IEditable
    {
        [SerializeField] private List<MaterialInventoryUI> inventoryUIList;


        public void ReloadAll()
        {
            inventoryUIList.ForEach(inventory => inventory.Reload());
        }

        public MaterialInventoryUI GetInventoryUI(DataIndex dataIndex)
            => inventoryUIList.TryGetElement(inventoryUI => inventoryUI.Category == dataIndex);


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
