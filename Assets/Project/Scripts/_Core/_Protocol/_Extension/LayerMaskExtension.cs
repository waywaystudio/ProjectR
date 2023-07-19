using System.Collections.Generic;
using UnityEngine;
// ReSharper disable UnusedMember.Global

public static class LayerMaskExtension
{
    /*
         * LayerMask layer; 
         * (int)layer, layer.value, LayerMask.GetMask("name") 모두 return flag.
         * LayerMask.NameToLayer("name"), LayerMask.LayerToName(index) 모두 return index;
         * 보통 기저함수에서 LayerMask를 Param으로 받는 경우, Index를 요구 함.
         * gameObject.layer -> index 값.
         */
    public static bool IsInLayerMask(this GameObject obj, LayerMask mask)
    {
        return (mask.value & (1 << obj.layer)) > 0;
    }

    public static int ToIndex(this LayerMask mask)
    {
        var maskValue = mask.value;
        
        // Check if the maskValue is not a power of 2
        if (maskValue != 0 && (maskValue & (maskValue - 1)) != 0)
        {
            Debug.LogWarning("LayerMask is not a power of 2, cannot be converted to a valid layer index.");
            return 0;
        }
        
        var index = 0;
        while ((maskValue >>= 1) > 0)
        {
            ++index;
        }
        
        return index;
    }
    
    public static List<int> ToIndexList(this LayerMask mask)
    {
        var result = new List<int>();
            
        for (var i = 0; i < 32; i++)
        {
            var layerMask = 1 << i;

            if ((mask.value & layerMask) != 0)
            {
                result.Add(i);
            }
        }

        return result;
    }

    /* In Project R */
    public static bool IsMonster(this GameObject obj) => obj.layer    == LayerMask.NameToLayer("Monster");
    public static bool IsAdventurer(this GameObject obj) => obj.layer == LayerMask.NameToLayer("Adventurer");
    public static int GetEnemyLayerIndex(this LayerMask layer)
    {
        if (layer.value == LayerMask.NameToLayer("Adventurer")) return LayerMask.NameToLayer("Monster");
        if (layer.value == LayerMask.NameToLayer("Monster")) return LayerMask.NameToLayer("Adventurer");
            
        Debug.LogWarning($"Layer must be Adventurer or Monster. Input:{LayerMask.LayerToName(layer)}");
        return 0;
    }
}