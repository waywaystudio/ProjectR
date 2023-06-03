using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.PartyCamps.Inventories
{
    public class ViceMaterialEditor : MonoBehaviour
    {
#if UNITY_EDITOR
        [Button(ButtonSizes.Large, ButtonStyle.Box)]
        public void AddAll100Material()
        {
            var inventory = GetComponent<ViceMaterialInventory>();
            
            ViceMaterialType.None.Iterator(viceType =>
            {
                if (viceType == ViceMaterialType.None) return;
                
                inventory.Add(viceType, 100);
            });
        }
#endif
    }
}
