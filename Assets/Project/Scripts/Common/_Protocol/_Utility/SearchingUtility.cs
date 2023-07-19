using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Common
{
    public static class SearchingUtility
    {
        public static void Sort(this List<ICombatTaker> list, Vector3 pivot, SortingType sortingType)
        {
            switch (sortingType)
            {
                case SortingType.None:               break;
                case SortingType.DistanceAscending:  SortByDistance(pivot, list); break;
                case SortingType.DistanceDescending: SortByDistance(pivot, list,true); break;
                case SortingType.HpAscending:        SortByHp(list); break;
                case SortingType.HpDescending:       SortByHp(list, true); break;
                case SortingType.HpRatioAscending:   SortByHpRatio(list); break;
                case SortingType.HpRatioDescending:  SortByHpRatio(list, true); break;
                case SortingType.CombatClass:        SortByCombatClass(list); break;
                case SortingType.Random:             SortByRandom(list); break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sortingType), sortingType, null);
            }
        }
        
        public static void SortByDistance(Vector3 pivot, List<ICombatTaker> targetList, bool isReverse = false)
        {
            targetList.Sort((x, y) =>
            {
                var xDistance = Vector3.Distance(pivot, x.gameObject.transform.position);
                var yDistance = Vector3.Distance(pivot, y.gameObject.transform.position);

                return !isReverse 
                    ? xDistance.CompareTo(yDistance) 
                    : yDistance.CompareTo(xDistance);
            });
        }
        
        public static void SortByHp(List<ICombatTaker> original, bool isReverse = false)
        {
            original.Sort((x, y) =>
            {
                var xHp = x.Hp.Value;
                var yHp = y.Hp.Value;

                return !isReverse 
                    ? xHp.CompareTo(yHp) 
                    : yHp.CompareTo(xHp);
            });
        }

        public static void SortByHpRatio(List<ICombatTaker> original, bool isReverse = false)
        {
            original.Sort((x, y) =>
            {
                var xHpRatio = x.Hp.Value / x.StatTable.MaxHp;
                var yHpRatio = y.Hp.Value / y.StatTable.MaxHp;

                return !isReverse 
                    ? xHpRatio.CompareTo(yHpRatio) 
                    : yHpRatio.CompareTo(xHpRatio);
            });
        }

        public static void SortByCombatClass(List<ICombatTaker> original, bool isReverse = false)
        {
            original.Sort((x, y) =>
            {
                var xRoleIndex = (int)x.CombatClass;
                var yRoleIndex = (int)y.CombatClass;

                return !isReverse 
                    ? xRoleIndex.CompareTo(yRoleIndex) 
                    : yRoleIndex.CompareTo(xRoleIndex);
            });
        }
        
        public static void SortByRandom(List<ICombatTaker> original)
        {
            var rnd = new Random();

            for (var i = original.Count - 1; i > 0; i--)
            {
                var randomIndex = rnd.Next(i);
                (original[i], original[randomIndex]) = (original[randomIndex], original[i]);
            }
        }

        public static void RangeFilter(this List<ICombatTaker> list, ICombatProvider provider, float range)
        {
            var pivot = provider.gameObject.transform.position;
            
            list.RemoveAll(x => Vector3.Distance(x.gameObject.transform.position, pivot) > range);
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
