using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Character.Targeting
{
    [RequireComponent(typeof(SphereCollider))]
    public class Searching : MonoBehaviour, IEditable
    {
        private const float SearchingRange = 100f;
        private const int MaxBufferCount = 32;
        
        [SerializeField] private SphereCollider searchingCollider;
        [SerializeField] private LayerMask adventurerLayer;
        [SerializeField] private LayerMask monsterLayer;
        
        private LayerMask selfLayer;

        public List<ICombatTaker> AdventurerList { get; } = new(MaxBufferCount);
        public List<ICombatTaker> MonsterList { get; } = new(MaxBufferCount);
        public ICombatTaker LookTarget => SetLookTarget(selfLayer);

        public ICombatTaker GetLookTarget(LayerMask layerIndex)
        {
            if (layerIndex == adventurerLayer)
            {
                return AdventurerList.HasElement()
                    ? AdventurerList[0]
                    : null;
            }
            if (layerIndex == monsterLayer)
            {
                return MonsterList.HasElement()
                    ? MonsterList[0]
                    : null;
            }

            return null;
        }

        
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
        

        private void Awake()
        {
            searchingCollider        ??= GetComponent<SphereCollider>();
            searchingCollider.radius =   SearchingRange;
            selfLayer                =   CharacterUtility.IndexToLayerValue(GetComponentInParent<ICombatProvider>().Object);
        }

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

        private ICombatTaker SetLookTarget(LayerMask self)
        {
            List<ICombatTaker> allyList;
            List<ICombatTaker> enemyList;

            if (self == adventurerLayer)
            {
                allyList  = AdventurerList;
                enemyList = MonsterList;
            }
            else /* if (self == monsterLayer) */
            {
                allyList  = MonsterList;
                enemyList = AdventurerList;
            }
            
            foreach (var t in enemyList) if (t.DynamicStatEntry.IsAlive.Value) return t;
            foreach (var t in allyList) if (t.DynamicStatEntry.IsAlive.Value) return t;

            return null;
        }

        private static bool IsAbleToCombat(Component other, LayerMask layer, out ICombatTaker taker)
            => other.TryGetComponent(out taker) && other.gameObject.IsInLayerMask(layer);
        

#if UNITY_EDITOR
        public void EditorSetUp()
        {
            searchingCollider        ??= GetComponent<SphereCollider>();
            searchingCollider.radius =   SearchingRange;
        }
#endif
    }
}