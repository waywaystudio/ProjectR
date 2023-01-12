using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Character.Combat.Projector.Colliders
{
    /* Collider Type 마다 셋팅 값이 다르다...
     Abstraction 할 수 밖에 없나; 
     이 친구는 Projector Object에 의해 기능이 결정된다. */
    public abstract class ProjectorCollider : MonoBehaviour, IEditorSetUp
    {
        [SerializeField] protected ProjectorObject po;
        
        [ShowInInspector] protected LayerMask TargetLayer;
        [ShowInInspector] protected Vector2 SizeValue;
        
        private int instanceID;
        protected const int MaxBuffer = 50;
        protected readonly Collider[] ColliderBuffer = new Collider[MaxBuffer];

        protected int InstanceID =>
            instanceID == 0
                ? instanceID = GetInstanceID()
                : instanceID;

        protected abstract void StartProjection();
        protected abstract void EndProjection();
        
        protected void OnTriggerEnter(Collider other)
        {
             if (other.gameObject.IsInLayerMask(TargetLayer) && 
                 other.gameObject.TryGetComponent(out ICombatTaker taker))
             {
                 po.OnProjectorEnter.Invoke(taker);
                 /* OnCollisionEntered :: Avoid */
                 // Try use Method 4: The ConstantPath type
                 // https://arongranberg.com/astar/documentation/dev_4_3_61_b7b7a3f3/wander.html
             }
        }

        protected virtual void Awake()
        {
            po          ??= GetComponentInParent<ProjectorObject>();
            TargetLayer =   po.TargetLayer;
            SizeValue   =   po.SizeValue;

            po.OnProjectionStart.Register(InstanceID, StartProjection);
            po.OnProjectionEnd.Register(InstanceID, EndProjection);
        }

#if UNITY_EDITOR
        public virtual void SetUp()
        {
            po ??= GetComponentInParent<ProjectorObject>();
        }
#endif
    }
}
