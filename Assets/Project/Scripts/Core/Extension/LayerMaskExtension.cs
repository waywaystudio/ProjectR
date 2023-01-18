using UnityEngine;

namespace Core
{
    public static class LayerMaskExtension
    {
        public static bool IsInLayerMask(this GameObject obj, LayerMask mask)
        {
            return (mask.value & (1 << obj.layer)) > 0;
        }
        
        public static bool Includes(this LayerMask mask, int layer)
        {
            return (mask.value & 1 << layer) > 0;
        }
        
        public static LayerMask Inverse(this LayerMask original)
        {
            return ~original;
        }

        public static int NameToLayerIndex(this LayerMask original, string layerName)
        {
            return LayerMask.NameToLayer(layerName);
        }
        
        public static int NameToLayerFlag(this LayerMask original, string layerName)
        {
            return 1 << LayerMask.NameToLayer(layerName);
        }

        public static LayerMask IndexToLayerValue(this int index)
        {
            if (index > 20)
            {
                Debug.LogError($"value must less then 20. input:{index}");
                return 0;
            }

            return 1 << index;
        }
    }
}
