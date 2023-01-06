using System.Collections.Generic;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Character.Targeting
{
    [RequireComponent(typeof(SphereCollider))]
    public class Searching : MonoBehaviour, ISearching, IEditorSetUp
    {
        private const float SearchingRange = 60f;
        private const int MaxBufferCount = 32;
        
        [SerializeField] private SphereCollider searchingCollider;
        
        private LayerMask adventurerLayer;
        private LayerMask monsterLayer;

        private readonly Collider[] colliderBuffer = new Collider[MaxBufferCount];

        [ShowInInspector] public List<ICombatTaker> AdventurerList { get; } = new(MaxBufferCount);
        [ShowInInspector] public List<ICombatTaker> MonsterList { get; } = new(MaxBufferCount);

        [ShowInInspector] 
        public ICombatTaker LookTarget => !MonsterList.IsNullOrEmpty() 
                ? MonsterList[0] 
                : !AdventurerList.IsNullOrEmpty() 
                    ? AdventurerList[0] 
                    : null;

        private List<ICombatTaker> OverlapListCache { get; } = new(MaxBufferCount);

        public List<ICombatTaker> OverlapList(LayerMask targetLayer, Vector3 center, float radius)
        {
            var hitCount = Physics.OverlapSphereNonAlloc(center, 
                radius, colliderBuffer, targetLayer);

#if UNITY_EDITOR
            if (hitCount >= MaxBufferCount)
                Debug.LogWarning($"Overflow Collider Max Buffer size : {MaxBufferCount}");          
#endif

            OverlapListCache.Clear();
            colliderBuffer.ForEach(x =>
            {
                if (x.IsNullOrEmpty()) return;
                if (!x.TryGetComponent(out ICombatTaker taker)) return;
                    
                OverlapListCache.Add(taker);
            });

            return OverlapListCache;
        }
        
        
        private void Awake()
        {
            var cb = GetComponentInParent<CharacterBehaviour>();

            cb.SearchingEngine       =   this;
            searchingCollider        ??= GetComponent<SphereCollider>();
            searchingCollider.radius =   SearchingRange;
            adventurerLayer          =   LayerMask.GetMask("Adventurer");
            monsterLayer             =   LayerMask.GetMask("Monster");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (IsAbleToCombatTake(other, adventurerLayer, out var adventurer))
            {
                AdventurerList.AddUniquely(adventurer);
#if UNITY_EDITOR
                if (AdventurerList.Count > MaxBufferCount)
                    Debug.LogWarning($"Oversize Adventurer added. current BufferSize : {MaxBufferCount}");
#endif
            }
            else if (IsAbleToCombatTake(other, monsterLayer, out var monster))
            {
                MonsterList.AddUniquely(monster);
#if UNITY_EDITOR
                if (MonsterList.Count > MaxBufferCount)
                    Debug.LogWarning($"Oversize Monster added. current BufferSize : {MaxBufferCount}");
#endif
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