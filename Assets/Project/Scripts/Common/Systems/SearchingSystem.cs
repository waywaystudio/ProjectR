using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Systems
{
    [RequireComponent(typeof(SphereCollider))]
    public class SearchingSystem : MonoBehaviour, IEditable
    {
        [SerializeField] private float searchingRange = 100f;
        [SerializeField] private SphereCollider searchingCollider;
        [SerializeField] private LayerMask targetLayerMask;
        
        public Table<LayerMask, List<ICombatTaker>> SearchedTable { get; } = new();
        
        
        [SerializeField] private LayerMask venturerLayer;
        [SerializeField] private LayerMask monsterLayer;
        private const int MaxBufferCount = 32;
        public List<ICombatTaker> AdventurerList { get; set; } = new(MaxBufferCount);
        public List<ICombatTaker> MonsterList { get; set; } = new(MaxBufferCount);
        public ICombatTaker GetMainTarget(LayerMask targetLayerIndex, Vector3 rootPosition, SortingType sortingType = SortingType.None)
        {
            if (targetLayerIndex == venturerLayer && AdventurerList.HasElement())
            {
                AdventurerList.SortingFilter(rootPosition, sortingType);
                
                foreach (var adventurer in AdventurerList)
                {
                    if (adventurer.DynamicStatEntry.Alive.Value) return adventurer;
                }
                
                return null;
            }

            if (targetLayerIndex == monsterLayer && MonsterList.HasElement())
            {
                MonsterList.SortingFilter(rootPosition, sortingType);

                foreach (var monster in MonsterList)
                {
                    if (monster.DynamicStatEntry.Alive.Value) return monster;
                }
                
                return null;
            }

            return null;
        }

        public void Clear()
        {
            AdventurerList.Clear();
            MonsterList.Clear();
        }


        private static bool IsAbleToCombat(Component other, LayerMask layer, out ICombatTaker taker)
        {
            return other.TryGetComponent(out taker) && other.gameObject.IsInLayerMask(layer);
        }

        private void Awake()
        {
            // Create Table
        }

        private void OnTriggerEnter(Collider other)
        {
            if (IsAbleToCombat(other, venturerLayer, out var adventurer))
            {
                if (!SearchedTable.ContainsKey(venturerLayer))
                {
                    SearchedTable.Add(venturerLayer, new List<ICombatTaker>());
                }
                
                SearchedTable[venturerLayer].AddUniquely(adventurer);
                AdventurerList.AddUniquely(adventurer);
            }
            else if (IsAbleToCombat(other, monsterLayer, out var monster))
            {
                if (!SearchedTable.ContainsKey(monsterLayer))
                {
                    SearchedTable.Add(monsterLayer, new List<ICombatTaker>());
                }
                
                SearchedTable[monsterLayer].AddUniquely(adventurer);
                MonsterList.AddUniquely(monster);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (IsAbleToCombat(other, venturerLayer, out var adventurer))
            {
                SearchedTable[venturerLayer]?.RemoveSafely(adventurer);
                AdventurerList.Remove(adventurer);
            }
            else if (IsAbleToCombat(other, monsterLayer, out var monster))
            {
                SearchedTable[monsterLayer]?.RemoveSafely(monster);
                MonsterList.Remove(monster);
            }
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            searchingCollider        ??= GetComponent<SphereCollider>();
            searchingCollider.radius =   searchingRange;
        }
#endif
    }
}