using System.Collections.Generic;
using Common.PartyCamps.Inventories;
using UnityEngine;
using UnityEngine.Serialization;

namespace Common.PartyCamps
{
    public class InventoryManager : MonoBehaviour, IEditable
    {
        [SerializeField] private ViceMaterialInventory viceMaterialInventory;

        // [Sirenix.OdinInspector.ShowInInspector]
        public Dictionary<ViceMaterialType, int> ViceMaterialTable => viceMaterialInventory.MaterialTable;
        
        // Material
        public int GetMaterialCount(ViceMaterialType type) => viceMaterialInventory.GetCount(type);
        public void AddMaterial(ViceMaterialType     type, int count) => viceMaterialInventory.Add(type, count);
        public void ConsumeMaterial(ViceMaterialType type, int count) => viceMaterialInventory.Consume(type, count);


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            viceMaterialInventory ??= GetComponentInChildren<ViceMaterialInventory>();
        }
#endif
        
    }
}
