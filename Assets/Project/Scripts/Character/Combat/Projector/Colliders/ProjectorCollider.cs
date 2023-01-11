using Core;
using UnityEngine;

namespace Character.Combat.Projector.Colliders
{
    public abstract class ProjectorCollider : MonoBehaviour, IEditorSetUp
    {
        [SerializeField] protected ProjectorObject projectorObject;
        [SerializeField] protected LayerMask targetLayer;
        [SerializeField] protected float sizeValue;

        private int instanceID;
        
        protected abstract void Generate(Vector3 providerPosition, Vector3 targetPosition);
        protected abstract void OnFinish();
        
        protected void OnCollisionEnter(Collision collisionInfo)
        {
            if (collisionInfo.gameObject.IsInLayerMask(targetLayer) && 
                collisionInfo.gameObject.TryGetComponent(out ICombatTaker taker))
            {
                /* OnCollisionEntered :: Avoid */
                // Try use Method 4: The ConstantPath type
                // https://arongranberg.com/astar/documentation/dev_4_3_61_b7b7a3f3/wander.html
            }
        }

        protected virtual void Awake()
        {
            instanceID      = GetInstanceID();
            projectorObject = GetComponentInParent<ProjectorObject>();
            // targetLayer     = projectorObject.TargetLayer;
            // sizeValue       = projectorObject.SizeValue;
            
            // projectorObject.OnGenerated.Register(instanceID, Generate);
            // projectorObject.OnFinished.Register(instanceID, OnFinish);
        }

#if UNITY_EDITOR
        public virtual void SetUp()
        {
            projectorObject ??= GetComponentInParent<ProjectorObject>();
            // targetLayer     =   projectorObject.TargetLayer;
            // sizeValue       =   projectorObject.SizeValue;
        }
#endif
    }
}
