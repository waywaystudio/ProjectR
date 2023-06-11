using System;
using UnityEngine;

namespace Common
{
    [Serializable]
    public class Ingredient<T> where T : struct
    {
        [SerializeField] protected T type;
        [SerializeField] protected int count;

        public T Type => type;
        public int Count => count;

        public Ingredient()
        {
            type  = default;
            count = 0;
        }
        public Ingredient(T type, int count)
        {
            this.type  = type;
            this.count = count;
        }
    }
    
    [Serializable] public class ViceIngredient : Ingredient<ViceMaterialType> { }
    [Serializable] public class GrowIngredient : Ingredient<GrowMaterialType>
    {
        public GrowIngredient(GrowMaterialType type, int count)
        {
            this.type  = type;
            this.count = count;
        }
    }
}
