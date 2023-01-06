using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Character.Targeting
{
    public class Targeting : MonoBehaviour, ITargeting
    {
        private ICombatTaker self;

        [Sirenix.OdinInspector.ShowInInspector]
        public ICombatTaker MainTarget { get; private set; }

        public ICombatTaker GetSelf()
        {
            MainTarget = self;
            return self;
        }
        
        public ICombatTaker GetTaker(List<ICombatTaker> targetList, ICombatProvider provider, float range, SortingType sortingType)
        {
            var inRanged = new List<ICombatTaker>();
            var pivot = provider.Object.transform.position;
            
            targetList.ForEach(x =>
            {
               if (Vector3.Distance(x.Object.transform.position, pivot) <= range)
                   inRanged.Add(x);
            });

            inRanged.SortingFilter(pivot, sortingType);
            
            MainTarget = inRanged.HasElement() 
                ? inRanged[0] 
                : null;

            return MainTarget;
        }


        private void Awake()
        {
            var cb = GetComponentInParent<CharacterBehaviour>();
            
            cb.TargetingEngine = this;
            self               = cb;
        }
    }
}
