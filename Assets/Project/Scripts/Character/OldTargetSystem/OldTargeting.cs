using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Character.TargetSystem
{
    public class OldTargeting : MonoBehaviour, ITargeting
    {
        private ICombatTaker self;

        public ICombatTaker MainTarget { get; private set; }

        public ICombatTaker GetSelf()
        {
            MainTarget = self;
            return self;
        }
        
        public ICombatTaker GetTaker(List<ICombatTaker> targetList, ICombatProvider provider, float range, SortingType sortingType)
        {
            var inRangedList = new List<ICombatTaker>();
            var pivot = provider.Object.transform.position;
            
            targetList.ForEach(x =>
            {
               if (Vector3.Distance(x.Object.transform.position, pivot) <= range)
                   inRangedList.Add(x);
            });

            inRangedList.SortingFilter(pivot, sortingType);

            MainTarget = MainTargeting(inRangedList);

            return MainTarget;
        }
        
        public List<ICombatTaker> GetTakerList(List<ICombatTaker> targetList, ICombatProvider provider, float range, SortingType sortingType, int count)
        {
            var inRangedList = new List<ICombatTaker>();
            var pivot = provider.Object.transform.position;
            
            targetList.ForEach(x =>
            {
                if (Vector3.Distance(x.Object.transform.position, pivot) <= range)
                    inRangedList.Add(x);
            });

            inRangedList.SortingFilter(pivot, sortingType);
            inRangedList.CountFilter(count);
            
            MainTarget = MainTargeting(inRangedList);

            return inRangedList;
        }
        
        public void SetTakerList(List<ICombatTaker> targetList, ICombatProvider provider, float range, SortingType sortingType, int count)
        {
            var pivot = provider.Object.transform.position;
            
            targetList.ForReverse(x =>
            {
                if (Vector3.Distance(x.Object.transform.position, pivot) > range) 
                    targetList.Remove(x);
            });

            targetList.RemoveNull();
            targetList.SortingFilter(pivot, sortingType);
            targetList.CountFilter(count);
        }


        private static ICombatTaker MainTargeting(List<ICombatTaker> preSortedList)
        {
            foreach (var t in preSortedList)
            {
                if (t.DynamicStatEntry.IsAlive.Value) return t;
            }

            return null;
        }

        private void Awake()
        {
            var cb = GetComponentInParent<CharacterBehaviour>();

            self = cb;
        }
    }
}
