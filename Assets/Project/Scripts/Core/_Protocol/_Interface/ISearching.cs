using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public interface ISearching
    {
        ICombatTaker LookTarget { get; }
        List<ICombatTaker> AdventurerList { get; }
        List<ICombatTaker> MonsterList { get; }
    }

    public interface ITargeting
    {
        ICombatTaker MainTarget { get; }
        ICombatTaker GetSelf();
        ICombatTaker GetTaker(List<ICombatTaker> targetList, ICombatProvider provider, float range,
            SortingType sortingType);
        
        List<ICombatTaker> GetTakerList(List<ICombatTaker> targetList, ICombatProvider provider, float range,
                                        SortingType sortingType, int count);
    }
}