using System;
using UnityEngine;

namespace Common
{
    [Serializable]
    public struct Ingredient
    {
        [SerializeField] private ViceMaterialType materialType;
        [SerializeField] private int count;

        public ViceMaterialType MaterialType => materialType;
        public int Count => count;

        public Ingredient(ViceMaterialType materialType = ViceMaterialType.None, int count = 0)
        {
            this.materialType = materialType;
            this.count        = count;
        }
    }
}
