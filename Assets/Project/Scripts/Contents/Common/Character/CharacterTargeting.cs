using System.Collections.Generic;
using System.Linq;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Character
{
    public class CharacterTargeting : MonoBehaviour
    {
        [SerializeField] private float searchingRange = 60f;
        [SerializeField] private LayerMask targetLayer;
        
        private const int MaxBufferCount = 100;
        private SphereCollider searchingCollider;
        private readonly Collider[] colliderBuffer = new Collider[MaxBufferCount];
        
        public float AttackRange { get; set; }
        public List<GameObject> SearchedTargets { get; } = new ();
        public List<GameObject> RangedTargets { get; } = new();
        [ShowInInspector] public GameObject FocusTarget { get; private set; }

        public void Initialize(float attackRange)
        {
            AttackRange = attackRange;
        }
        
        /// <summary>
        /// Call On Update Event Function
        /// </summary>
        public void UpdateTargeting()
        {
            UpdateRangedTargets(SearchedTargets);
            UpdateFocusTarget(SearchedTargets, RangedTargets);
        }
        
        /// <summary>
        /// Call When Target's activation changed. ex Monster Dead.
        /// </summary>
        public void UpdateSearchingTargets()
        {
            var hitCount = Physics.OverlapSphereNonAlloc(transform.position, searchingRange, colliderBuffer, targetLayer);

#if UNITY_EDITOR
            if (hitCount >= MaxBufferCount)
                Debug.LogWarning($"Overflow Collider Max Buffer size : {MaxBufferCount}");          
#endif
            
            SearchedTargets.Clear();
            colliderBuffer.ForEach(x =>
            {
                if (x.IsNullOrEmpty()) return;
                SearchedTargets.Add(x.gameObject);
            });
        }

        private void Awake()
        {
            searchingCollider = GetComponent<SphereCollider>();
            searchingCollider.radius = searchingRange;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.IsInLayerMask(targetLayer)) return;

            SearchedTargets.AddUniquely(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.IsInLayerMask(targetLayer)) return;

            SearchedTargets.RemoveSafely(other.gameObject);
        }

        private void UpdateRangedTargets(List<GameObject> searchedTargets)
        {
            RangedTargets.Clear();
            
            var selfPosition = transform.position;
            searchedTargets.ForEach(x =>
            {
                if (x == null || x.activeSelf == false) return;
                if (Vector3.Distance(x.transform.position, selfPosition) <= AttackRange)
                {
                    RangedTargets.Add(x);
                }
            });
        }

        private void UpdateFocusTarget(List<GameObject> searchedTargets, List<GameObject> rangeTargets)
        {
            if (rangeTargets.IsNullOrEmpty())
            {
                FocusTarget = searchedTargets.IsNullOrEmpty() ? null 
                                                              : searchedTargets.First();
            }
            else
                FocusTarget = rangeTargets.First();
        }

        private void OnDrawGizmos()
        {
            if (RangedTargets.IsNullOrEmpty()) return;
            
            Gizmos.color = new Color(129, 0, 0, 0.3f);
            Gizmos.DrawSphere(transform.position, AttackRange);
        }

        #region LEGACY
        /*
         * 고정배열 쓰기가 좀 어렵다.
         */
        // [ShowInInspector] private Collider[] searchedTargetList;
        // [ShowInInspector] private readonly List<Collider> rangedTargetList = new ();
        // [ShowInInspector] private Collider lookTarget;
        //
        // public void Searching(float searchRadius, float range)
        // {
        //     GetSearchedTargetList(searchRadius);
        //     GetRangedTargetList(range);
        //     GetLookTarget();
        // }
        //
        //
        // private void GetSearchedTargetList(float searchRadius)
        // {
        //     searchedTargetList ??= new Collider[maxTargetCount];
        //     Array.Clear(searchedTargetList, 0, 10);
        //
        //     Physics.OverlapSphereNonAlloc(transform.position, 
        //                                   searchRadius,
        //                                   searchedTargetList, 
        //                                   targetLayer);
        // }
        //
        // private void GetRangedTargetList(float range)
        // {
        //     if (searchedTargetList.IsNullOrEmpty()) return;
        //     
        //     rangedTargetList.Clear();
        //     searchedTargetList.ForEach(x =>
        //     {
        //         if (x && Vector3.Distance(x.transform.position, transform.position) <= range)
        //         {
        //             // x.gameObject.GetComponent<IInterface>();
        //             // rangedTargetList.Add(IInterface);
        //             rangedTargetList.Add(x);
        //         }
        //     });
        // }
        //
        // private void GetLookTarget()
        // {
        //     lookTarget = rangedTargetList.FirstOrDefault();
        // }
        
        /*
         * 마찬가지로 고정배열 쓰기가 좀 어렵다.
         */
        // private void UpdateSearching()
        // {
        //     searchedList.Clear();
        //     var colliders = Physics.OverlapSphere(transform.position, searchingRange).ToList();
        //     
        //     colliders.ForEach(x =>
        //     {
        //         if (!x.CompareTag(Monster))
        //         {
        //             colliders.RemoveSafely(x);
        //             return;
        //         }
        //
        //         searchedList.Add(x.gameObject);
        //     });
        // }

        #endregion
    }
}
