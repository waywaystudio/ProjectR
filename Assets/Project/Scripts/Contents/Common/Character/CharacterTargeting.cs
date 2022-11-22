using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Character
{
    public class CharacterTargeting : MonoBehaviour
    {
        [SerializeField] private float searchingRadius;
        [SerializeField] private float attackRadius;
        [SerializeField] private LayerMask targetLayer;

        [ShowInInspector]
        private Collider[] searchedColliders = new Collider[1];
        [ShowInInspector]
        private Collider[] aimedColliders = new Collider[1];
        [ShowInInspector] private int searchedCount = 0;
        [ShowInInspector] private int aimedCount = 0;

        private void Update()
        {
            var hit = Physics.OverlapSphereNonAlloc(transform.position, searchingRadius, searchedColliders, targetLayer);
            var withIn = Physics.OverlapSphereNonAlloc(transform.position, attackRadius, aimedColliders, targetLayer);

            if (hit != searchedCount)
            {
                searchedCount = hit;
                Debug.Log($"Hit Count : {searchedCount}");
            }
            
            if (withIn != aimedCount)
            {
                aimedCount = withIn;
                Debug.Log($"Aimed Count : {aimedCount}");
            }
        }

        [Button]
        private void Collider()
        {
            Debug.Log(searchedColliders.Length);
        }
    }
}
