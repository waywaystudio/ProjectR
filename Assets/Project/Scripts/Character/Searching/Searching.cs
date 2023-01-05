using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Character.Searching
{
    [RequireComponent(typeof(SphereCollider))]
    public class Searching : MonoBehaviour, ISearchEngine, IEditorSetUp
    {
        [SerializeField] private float searchingRange;
        [SerializeField] private SphereCollider searchingCollider;
        [SerializeField] private LayerMask adventurerLayer;
        [SerializeField] private LayerMask monsterLayer;
        
        private const int MaxBufferCount = 100;
        private readonly Collider[] colliderBuffer = new Collider[MaxBufferCount];
        private float SearchingRange => searchingRange;

        public List<ICombatTaker> AdventurerList { get; } = new();
        public List<ICombatTaker> MonsterList { get; } = new();

        // 현재 Searching 의 실제 기능은 OnTrigger 밖에 없다.
        // 핵심은 UpdateTarget() 이며 필요할 때 호출해보도록 하자.
        public void UpdateList()
        {
            UpdateCharacterList();
            UpdateMonsterList();
        }

        public void UpdateCharacterList() => UpdateTargetList(adventurerLayer, AdventurerList);
        public void UpdateMonsterList() => UpdateTargetList(monsterLayer, MonsterList);
        

        private void UpdateTargetList(LayerMask targetLayer, ICollection<ICombatTaker> output)
        {
            var hitCount = Physics.OverlapSphereNonAlloc(transform.position, 
                SearchingRange, colliderBuffer, targetLayer);

#if UNITY_EDITOR
            if (hitCount >= MaxBufferCount)
                Debug.LogWarning($"Overflow Collider Max Buffer size : {MaxBufferCount}");          
#endif

            output.Clear();
            colliderBuffer.ForEach(x =>
            {
                if (x.IsNullOrEmpty()) return;
                output.Add(x.GetComponent<ICombatTaker>());
            });
        }
        
        private void Awake()
        {
            var cb = GetComponentInParent<CharacterBehaviour>();
            cb.AdventurerList = AdventurerList;
            cb.MonsterList = MonsterList;
            
            searchingCollider ??= GetComponent<SphereCollider>();
            searchingCollider.radius = SearchingRange;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (IsAbleToCombatTake(other, adventurerLayer, out var adventurer))
            {
                AdventurerList.AddUniquely(adventurer);
            }
            else if (IsAbleToCombatTake(other, monsterLayer, out var monster))
            {
                MonsterList.AddUniquely(monster);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (IsAbleToCombatTake(other, adventurerLayer, out var adventurer))
            {
                AdventurerList.RemoveSafely(adventurer);
            }
            else if (IsAbleToCombatTake(other, monsterLayer, out var monster))
            {
                MonsterList.RemoveSafely(monster);
            }
        }

        private static bool IsAbleToCombatTake(Component other, LayerMask layer, out ICombatTaker taker)
            => other.TryGetComponent(out taker) && other.gameObject.IsInLayerMask(layer);

#if UNITY_EDITOR
        public void SetUp()
        {
            if (searchingRange == 0)
                searchingRange = 50f;

            searchingCollider ??= GetComponent<SphereCollider>();
            adventurerLayer = LayerMask.GetMask("Adventurer");
            monsterLayer = LayerMask.GetMask("Monster");
        }
#endif
    }
}