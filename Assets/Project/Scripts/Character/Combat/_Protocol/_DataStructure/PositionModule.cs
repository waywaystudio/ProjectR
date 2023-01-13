using UnityEngine;

namespace Character.Combat
{
    public class PositionModule : Module
    {
        [SerializeField] private bool isRandom;
        [SerializeField] private float randomRadius;
        [SerializeField] private float positionCount;

        private LayerMask targetLayer;
        private Vector3 targetPosition;

        public void Fire(Vector3 position)
        {
            
        }

        public void Projection(Vector3 position)
        {
            
        }

        public void Put(Vector3 position)
        {
            
        }
    }
}
