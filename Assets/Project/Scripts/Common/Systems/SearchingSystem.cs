using System.Collections.Generic;
using UnityEngine;

namespace Common.Systems
{
    [RequireComponent(typeof(SphereCollider))]
    public class SearchingSystem : MonoBehaviour, IEditable
    {
        [SerializeField] private float searchingRange = 50f;
        [SerializeField] private SphereCollider searchingCollider;
        
        private const int MaxBufferCount = 32;
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

        public ICombatTaker GetSelf() => GetComponentInParent<ICombatExecutor>();

        // TODO. Adventurer를 Main에서 사용할 예정이라면, 씬이 변경될 때 해제해 주어야 하는 파트를 고려해야 한다.
        public void Clear()
        {
            AdventurerList.Clear();
            MonsterList.Clear();
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
            searchingCollider.radius =   searchingRange;
            adventurerLayer          =   LayerMask.GetMask("Adventurer");
            monsterLayer             =   LayerMask.GetMask("Monster");
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