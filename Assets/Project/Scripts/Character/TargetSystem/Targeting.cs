using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Character.TargetSystem
{
    public class Targeting : MonoBehaviour, ITargeting
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
            
            MainTarget = inRangedList.HasElement() 
                ? inRangedList[0] 
                : null;

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
            
            MainTarget = inRangedList.HasElement() 
                                 ? inRangedList[0] 
                                 : null;

            return inRangedList;
        }
        

        private void Awake()
        {
            var cb = GetComponentInParent<CharacterBehaviour>();
            
            cb.TargetingEngine = this;
            self               = cb;
        }
    }
}
