using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Common.Character
{
    [RequireComponent(typeof(SphereCollider))]
    public class Searching : MonoBehaviour
    {
        [SerializeField] private CharacterBehaviour cb;
        [SerializeField] private SphereCollider searchingCollider;
        
        private const int MaxBufferCount = 100;
        private readonly Collider[] colliderBuffer = new Collider[MaxBufferCount];
        private float SearchingRange => cb.SearchingRange;

        public void UpdateTarget()
        {
            UpdateCharacterList();
            UpdateMonsterList();
        }

        public void UpdateCharacterList()
        {
            UpdateSearchingList(cb.AllyLayer, cb.CharacterSearchedList);
        }

        public void UpdateMonsterList()
        {
            UpdateSearchingList(cb.EnemyLayer, cb.MonsterSearchedList);
        }

        private void Awake()
        {
            cb ??= GetComponentInParent<CharacterBehaviour>();
            searchingCollider ??= GetComponent<SphereCollider>();
            searchingCollider.radius = SearchingRange;
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.IsInLayerMask(cb.AllyLayer))
            {
                cb.CharacterSearchedList.AddUniquely(other.gameObject);
            } 
            else if (other.gameObject.IsInLayerMask(cb.EnemyLayer))
            {
                cb.MonsterSearchedList.AddUniquely(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.IsInLayerMask(cb.AllyLayer))
            {
                cb.CharacterSearchedList.RemoveSafely(other.gameObject);
            } 
            else if (other.gameObject.IsInLayerMask(cb.EnemyLayer))
            {
                cb.MonsterSearchedList.RemoveSafely(other.gameObject);
            }
        }
    }
}

// private void UpdateFocusTarget(List<GameObject> searchedTargets, List<GameObject> rangeTargets)
// {
//     if (rangeTargets.IsNullOrEmpty())
//     {
//         FocusTarget = searchedTargets.IsNullOrEmpty() ? null 
//                                                       : searchedTargets.First();
//     }
//     else
//         FocusTarget = rangeTargets.First();
// }
// private void OnDrawGizmos()
// {
//     if (RangedTargets.IsNullOrEmpty()) return;
//     
//     Gizmos.color = new Color(129, 0, 0, 0.3f);
//     Gizmos.DrawSphere(transform.position, cb.Range);
// }