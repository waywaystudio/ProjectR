using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Character.TargetSystem
{
    [RequireComponent(typeof(SphereCollider))]
    public class Searching : MonoBehaviour, ISearching, IInspectorSetUp
    {
        private const float SearchingRange = 100f;
        private const int MaxBufferCount = 32;
        
        [SerializeField] private SphereCollider searchingCollider;
        [SerializeField] private SortingType sortingType;
        
        private LayerMask adventurerLayer;
        private LayerMask monsterLayer;
        private LayerMask selfLayer;

        public List<ICombatTaker> AdventurerList { get; } = new(MaxBufferCount);
        public List<ICombatTaker> MonsterList { get; } = new(MaxBufferCount);
        public ICombatTaker LookTarget => SetLookTarget(selfLayer);

        public ICombatTaker GetLookTarget(LayerMask layer)
        {
            if (layer == adventurerLayer)
            {
                return AdventurerList.HasElement()
                    ? AdventurerList[0]
                    : null;
            }
            if (layer == monsterLayer)
            {
                return MonsterList.HasElement()
                    ? MonsterList[0]
                    : null;
            }

            return null;
        }


        private void Awake()
        {
            searchingCollider        ??= GetComponent<SphereCollider>();
            searchingCollider.radius =   SearchingRange;
            adventurerLayer          =   LayerMask.GetMask("Adventurer");
            monsterLayer             =   LayerMask.GetMask("Monster");
            selfLayer                =   CharacterUtility.IndexToLayerValue(GetComponentInParent<ICombatProvider>().Object);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (IsAbleToCombatTake(other, adventurerLayer, out var adventurer))
            {
                AdventurerList.AddUniquely(adventurer);
                AdventurerList.SortingFilter(transform.position, sortingType);
            }
            else if (IsAbleToCombatTake(other, monsterLayer, out var monster))
            {
                MonsterList.AddUniquely(monster);
                MonsterList.SortingFilter(transform.position, sortingType);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (IsAbleToCombatTake(other, adventurerLayer, out var adventurer))
            {
                AdventurerList.Remove(adventurer);
            }
            else if (IsAbleToCombatTake(other, monsterLayer, out var monster))
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

        private static bool IsAbleToCombatTake(Component other, LayerMask layer, out ICombatTaker taker)
            => other.TryGetComponent(out taker) && other.gameObject.IsInLayerMask(layer);
        

#if UNITY_EDITOR
        public void SetUp()
        {
            searchingCollider        ??= GetComponent<SphereCollider>();
            searchingCollider.radius =   SearchingRange;
            adventurerLayer          =   LayerMask.GetMask("Adventurer");
            monsterLayer             =   LayerMask.GetMask("Monster");
        }
#endif
    }
}