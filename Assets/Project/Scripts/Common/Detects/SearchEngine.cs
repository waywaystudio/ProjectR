using System.Collections.Generic;
using UnityEngine;

namespace Common.Detects
{
    public class SearchEngine : MonoBehaviour, IEditable
    {
        [SerializeField] private float searchingRange = 100f;
        [SerializeField] private SphereCollider searchingCollider;
        [SerializeField] private GameObject RootObject;
        [SerializeField] private LayerMask targetLayerMask;

        public Dictionary<int, List<GameObject>> SearchedTable { get; } = new();


        private void Awake()
        {
            searchingCollider.radius = searchingRange;

            for (var i = 0; i < 32; i++)
            {
                var layerMask = 1 << i;

                if ((targetLayerMask.value & layerMask) != 0)
                {
                    SearchedTable.Add(i, new List<GameObject>());
                }
            }
            
            var rootLayer = RootObject.layer;
            
            if (!RootObject.IsNullOrEmpty())
            {
                SearchedTable[rootLayer].AddUniquely(RootObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var enteredObject = other.gameObject;
            var enteredComponentLayer = enteredObject.layer;

            if (!enteredObject.IsInLayerMask(targetLayerMask)) return;

            SearchedTable[enteredComponentLayer]?.Add(enteredObject);
        }
        
        private void OnTriggerExit(Collider other)
        {
            var enteredObject = other.gameObject;
            var enteredComponentLayer = enteredObject.layer;

            SearchedTable[enteredComponentLayer]?.RemoveSafely(enteredObject);
        }
        
        
#if UNITY_EDITOR
        public void EditorSetUp()
        {
            searchingCollider        = GetComponent<SphereCollider>();
            searchingCollider.radius = searchingRange;
        }
#endif
    }
}
