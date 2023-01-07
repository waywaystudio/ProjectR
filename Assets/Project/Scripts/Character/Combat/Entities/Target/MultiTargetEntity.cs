using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Character.Combat.Entities.Target
{
    // public class MultiTargetEntity : TargetEntity
    // {
    //     [SerializeField] private int targetCount;
    //
    //     private List<ICombatTaker> lookTargetCache = new(1);
    //
    //     public override bool IsReady => Targets.HasElement();
    //     public List<ICombatTaker> Targets
    //     {
    //         get
    //         {
    //             var targetList = TargetingEngine.GetTakerList(TargetList, Provider, Range, sortingType, targetCount);
    //
    //             if (targetList.IsNullOrEmpty())
    //             {
    //                 lookTargetCache.Clear();
    //                 lookTargetCache.Add(SearchingEngine.LookTarget);
    //
    //                 targetList = lookTargetCache;
    //             }
    //
    //             return targetList;
    //         }
    //     }
    // }
}
