using System;
using UnityEngine;

namespace Common
{
    [Serializable]
    public struct Ingredient
    {
        [SerializeField] private MaterialType materialType;
        [SerializeField] private int count;

        public MaterialType MaterialType => materialType;
        public int Count => count;

        public Ingredient(MaterialType materialType = MaterialType.None, int count = 0)
        {
            this.materialType = materialType;
            this.count        = count;
        }
    }
}
