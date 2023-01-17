using Core;
using UnityEngine;

namespace Character.Combat.Projector.Colliders
{
    /* Collider Type 마다 셋팅 값이 다르다...
     Abstraction 할 수 밖에 없나; 
     이 친구는 Projector Object에 의해 기능이 결정된다. */
    public abstract class ProjectorCollider : MonoBehaviour, IInspectorSetUp
    {
        [SerializeField] protected ProjectorObject po;
        
        protected LayerMask TargetLayer;
        protected Vector2 SizeValue;
        
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
             }
        }

        protected virtual void Awake()
        {
            po        ??= GetComponentInParent<ProjectorObject>();
            SizeValue =   po.SizeValue;

            po.OnActivated.Register(InstanceID, StartProjection);
            po.OnCompleted.Register(InstanceID, EndProjection);
        }

#if UNITY_EDITOR
        public virtual void SetUp()
        {
            po ??= GetComponentInParent<ProjectorObject>();
        }
#endif
    }
}
