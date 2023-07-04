using System;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Venturers.Rogue
{
    [CreateAssetMenu(menuName = "ScriptableObject/PhantomMaster", fileName = "PhantomMaster")]
    public class PhantomMaster : ScriptableObject
    {
        private readonly List<PhantomTrap> phantomList = new();

        public void DoubleStab(Vector3 targetPosition, float haste)
        {
            phantomList.ForEach(phantom =>
            {
                if (!phantom.gameObject.activeSelf) return;

                phantom.DoubleStab(targetPosition, haste);
            });
        }

        public void MarkOfDeath(Vector3 targetPosition, float haste)
        {
            phantomList.ForEach(phantom =>
            {
                if (!phantom.gameObject.activeSelf) return;

                phantom.MarkOfDeath(targetPosition, haste);
            });
        }

        public void Add(PhantomTrap phantom)
        {
            phantomList.AddUniquely(phantom);
        }

        public void Remove(PhantomTrap phantom)
        {
            phantomList.RemoveSafely(phantom);
        }

        public void Clear() => phantomList.Clear();
    }
}
