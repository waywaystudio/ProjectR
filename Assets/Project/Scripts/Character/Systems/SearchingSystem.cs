using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Character.Systems
{
    [RequireComponent(typeof(SphereCollider))]
    public class SearchingSystem : MonoBehaviour, IEditable
    {
        private const float SearchingRange = 50f;
        private const int MaxBufferCount = 32;
        
        [SerializeField] private SphereCollider searchingCollider;

        private LayerMask adventurerLayer;
        private LayerMask monsterLayer;

        public List<ICombatTaker> AdventurerList { get; } = new(MaxBufferCount);
        public List<ICombatTaker> MonsterList { get; } = new(MaxBufferCount);

        public ICombatTaker GetMainTarget(LayerMask targetLayerIndex, Vector3 rootPosition, SortingType sortingType = SortingType.None)
        {
            (targetLayerIndex == adventurerLayer || targetLayerIndex == monsterLayer).OnFalse(() 
                => Debug.LogWarning($"Layer must be Adventurer or Monster. Input:{(int)targetLayerIndex}"));
            
            if (targetLayerIndex == adventurerLayer && AdventurerList.HasElement())
            {
                AdventurerList.SortingFilter(rootPosition, sortingType);
                
                foreach (var adventurer in AdventurerList)
                {
                    if (adventurer.DynamicStatEntry.IsAlive.Value) return adventurer;
                }
                
                return null;
            }

            if (targetLayerIndex == monsterLayer && MonsterList.HasElement())
            {
                MonsterList.SortingFilter(rootPosition, sortingType);

                foreach (var monster in MonsterList)
                {
                    if (monster.DynamicStatEntry.IsAlive.Value) return monster;
                }
                
                return null;
            }

            return null;
        }


        private static bool IsAbleToCombat(Component other, LayerMask layer, out ICombatTaker taker)
            => other.TryGetComponent(out taker) && other.gameObject.IsInLayerMask(layer);


        private void OnTriggerEnter(Collider other)
        {
            if (IsAbleToCombat(other, adventurerLayer, out var adventurer))
            {
                AdventurerList.AddUniquely(adventurer);
            }
            else if (IsAbleToCombat(other, monsterLayer, out var monster))
            {
                MonsterList.AddUniquely(monster);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (IsAbleToCombat(other, adventurerLayer, out var adventurer))
            {
                AdventurerList.Remove(adventurer);
            }
            else if (IsAbleToCombat(other, monsterLayer, out var monster))
            {
                MonsterList.Remove(monster);
            }
        }
        
        private void Awake()
        {
            searchingCollider        ??= GetComponent<SphereCollider>();
            searchingCollider.radius =   SearchingRange;
            adventurerLayer          =   LayerMask.GetMask("Adventurer");
            monsterLayer             =   LayerMask.GetMask("Monster");
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            searchingCollider        ??= GetComponent<SphereCollider>();
            searchingCollider.radius =   SearchingRange;
        }
#endif
    }
}