using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Common.Character.Operation.Combat
{
    [RequireComponent(typeof(SphereCollider))]
    public class Searching : MonoBehaviour
    {
        [SerializeField] private CharacterBehaviour cb;
        [SerializeField] private SphereCollider searchingCollider;
        
        private const int MaxBufferCount = 100;
        private readonly Collider[] colliderBuffer = new Collider[MaxBufferCount];
        private float SearchingRange => cb.SearchingRange;

        // 현재 Searching 의 실제 기능은 OnTrigger 밖에 없다.
        // 핵심은 UpdateTarget() 이며 필요할 때 호출해보도록 하자.
        public void UpdateTarget()
        {
            UpdateCharacterList();
            UpdateMonsterList();
        }

        public void UpdateCharacterList()
        {
            UpdateSearchingList(cb.AllyLayer, cb.AdventureList);
        }

        public void UpdateMonsterList()
        {
            UpdateSearchingList(cb.EnemyLayer, cb.MonsterList);
        }
        

        private void UpdateSearchingList(LayerMask targetLayer, ICollection<GameObject> output)
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
                output.Add(x.gameObject);
            });
        }
        
        private void Awake()
        {
            cb ??= GetComponentInParent<CharacterBehaviour>();
            searchingCollider ??= GetComponent<SphereCollider>();
            searchingCollider.radius = SearchingRange;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.IsInLayerMask(cb.AllyLayer))
            {
                cb.AdventureList.AddUniquely(other.gameObject);
            } 
            else if (other.gameObject.IsInLayerMask(cb.EnemyLayer))
            {
                cb.MonsterList.AddUniquely(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.IsInLayerMask(cb.AllyLayer))
            {
                cb.AdventureList.RemoveSafely(other.gameObject);
            } 
            else if (other.gameObject.IsInLayerMask(cb.EnemyLayer))
            {
                cb.MonsterList.RemoveSafely(other.gameObject);
            }
        }
    }
}