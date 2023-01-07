using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Character.TargetSystem
{
    [RequireComponent(typeof(SphereCollider))]
    public class Searching : MonoBehaviour, ISearching, IEditorSetUp
    {
        private const float SearchingRange = 60f;
        private const int MaxBufferCount = 32;
        
        [SerializeField] private SphereCollider searchingCollider;
        
        private LayerMask adventurerLayer;
        private LayerMask monsterLayer;

        public List<ICombatTaker> AdventurerList { get; } = new(MaxBufferCount);
        public List<ICombatTaker> MonsterList { get; } = new(MaxBufferCount);
        public ICombatTaker LookTarget => !MonsterList.IsNullOrEmpty() 
                ? MonsterList[0] 
                : !AdventurerList.IsNullOrEmpty() 
                    ? AdventurerList[0] 
                    : null;


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