using UnityEngine;

namespace Core
{
    public static class LayerMaskExtension
    {
        /*
         * LayerMask layer;
         * layer, layer.value, LayerMask.GetMask("name") 모두 return flag.
         * LayerMask.NameToLayer("name"), LayerMask.LayerToName(index) 모두 return index;
         * gameObject.layer -> index 값.
         */
        public static bool IsInLayerMask(this GameObject obj, LayerMask mask)
        {
            return (mask.value & (1 << obj.layer)) > 0;
        }
        
        /* In Project R */
        public static bool IsMonster(this GameObject obj) => obj.layer == LayerMask.NameToLayer("Monster");
        public static bool IsAdventurer(this GameObject obj) => obj.layer == LayerMask.NameToLayer("Adventurer");
        public static int GetEnemyLayerIndex(this LayerMask layer)
        {
            if (layer.value == LayerMask.NameToLayer("Adventurer")) return LayerMask.NameToLayer("Monster");
            if (layer.value == LayerMask.NameToLayer("Monster")) return LayerMask.NameToLayer("Adventurer");
            
            Debug.LogWarning($"layer must be Adventurer or Monster. Input:{LayerMask.LayerToName(layer)}");
            return 0;
        }
    }
}
