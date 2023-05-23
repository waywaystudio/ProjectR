using System.Collections.Generic;
using Common.PartyCamps.Inventories;
using UnityEngine;

namespace Common.PartyCamps
{
    public class InventoryManager : MonoBehaviour, IEditable
    {
        [SerializeField] private MaterialInventory materialInventory;

        [Sirenix.OdinInspector.ShowInInspector]
        public Dictionary<MaterialType, int> MaterialTable => materialInventory.MaterialTable;
        
        // Material
        public int GetMaterialCount(MaterialType type) => materialInventory.GetCount(type);
        public void AddMaterial(MaterialType     type, int count) => materialInventory.Add(type, count);
        public void ConsumeMaterial(MaterialType type, int count) => materialInventory.Consume(type, count);


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            materialInventory ??= GetComponentInChildren<MaterialInventory>();
        }
#endif
        
    }
}
