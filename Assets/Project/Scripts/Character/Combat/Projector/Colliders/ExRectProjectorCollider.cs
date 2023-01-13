using Core;
using UnityEngine;

namespace Character.Combat.Projector.Colliders
{
    public class ExRectProjectorCollider : ProjectorCollider
    {
        [SerializeField] private BoxCollider boxCollider;
        
        private const float BoxHeight = 2f;
        private const float ExtendLength = 100f;
        private Vector3 halfExtendBuffer;
        private Quaternion orientationBuffer;

        protected override void StartProjection()
        {
            var providerPosition = po.Provider.Object.transform.position;
            var takerPosition = po.Taker != null
                ? po.Taker.Object.transform.position
                : po.Destination;
            
            TargetLayer           = po.TargetLayer;
            po.transform.position = providerPosition;
            boxCollider.size      = GenerateDynamicSize();

            LookAt(takerPosition, out orientationBuffer);
        }

        protected override void EndProjection()
        {
            Physics.OverlapBoxNonAlloc(transform.position, halfExtendBuffer, ColliderBuffer, orientationBuffer, TargetLayer);

            if (ColliderBuffer.IsNullOrEmpty()) return;
            
            ColliderBuffer.ForEach(x =>
            {
                if (x.IsNullOrEmpty()) return;
                if (!x.TryGetComponent(out ICombatTaker taker)) return;
                
                po.OnProjectorEnd.Invoke(taker);
            });
        }
        
        private Vector3 GenerateDynamicSize()
        {
            var result = new Vector3(SizeValue.x,BoxHeight, ExtendLength);

            halfExtendBuffer = result * 0.5f;

            return result;
        }
        
        private void LookAt(Vector3 takerPosition, out Quaternion buffer)
        {
            var poTransform = po.transform;
            poTransform.LookAt(takerPosition);

            buffer = poTransform.rotation;
        }
        
        
        protected override void Awake()
        {
            base.Awake();

            boxCollider ??= GetComponent<BoxCollider>();
        }
        
#if UNITY_EDITOR
        public override void SetUp()
        {
            base.SetUp();
            
            boxCollider = GetComponent<BoxCollider>();
        }
#endif
    }
}
