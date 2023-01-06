using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Character
{
    public static class SearchingUtility
    {
        public static void SortByDistance(Vector3 pivot, List<ICombatTaker> targetList, bool isReverse = false)
        {
            targetList.Sort((x, y) =>
            {
                var xDistance = Vector3.Distance(pivot, x.Object.transform.position);
                var yDistance = Vector3.Distance(pivot, y.Object.transform.position);

                return !isReverse 
                    ? xDistance.CompareTo(yDistance) 
                    : yDistance.CompareTo(xDistance);
            });
        }
        
        public static void SortByHp(List<ICombatTaker> original, bool isReverse = false)
        {
            original.Sort((x, y) =>
            {
                var xHp = x.DynamicStatEntry.Hp.Value;
                var yHp = y.DynamicStatEntry.Hp.Value;

                return !isReverse 
                    ? xHp.CompareTo(yHp) 
                    : yHp.CompareTo(xHp);
            });
        }
        
        public static void RangeFilter(this List<ICombatTaker> list, ICombatProvider provider, float range)
        {
            var pivot = provider.Object.transform.position;
            
            list.RemoveAll(x => Vector3.Distance(x.Object.transform.position, pivot) > range);
        }

        public static void SortingFilter(this List<ICombatTaker> list, Vector3 pivot, SortingType sortingType)
        {
            switch (sortingType)
            {
                case SortingType.None: break;
                case SortingType.DistanceAscending: SortByDistance(pivot, list); break;
                case SortingType.DistanceDescending:SortByDistance(pivot, list,true); break;
                case SortingType.HpAscending: SortByHp(list); break;
                case SortingType.HpDescending: SortByHp(list, true); break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sortingType), sortingType, null);
            }
        }

        public static void CountFilter(this List<ICombatTaker> list, int count)
        {
            list.Trim(count);
        }

        public static LayerMask GetEnemyLayerMask(this LayerMask mask)
        {
            var adventurerLayer = LayerMask.GetMask("Adventurer");
            var monsterLayer = LayerMask.GetMask("Monster");

            if (mask == adventurerLayer) return monsterLayer;
            if (mask == monsterLayer) return adventurerLayer;
        
            Debug.LogError($"Can't Get EnemyMask. Input Must be Adventurer or Monster. Input:{mask}");
            return 0;
        }
    }
}
