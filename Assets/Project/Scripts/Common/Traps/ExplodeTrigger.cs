using UnityEngine;

namespace Common.Traps
{
    [RequireComponent(typeof(SphereCollider))]
    public class ExplodeTrigger : MonoBehaviour, IEditable
    {
        [SerializeField] protected SphereCollider triggerCollider;
        
        private LayerMask targetLayer;
        private TrapComponent trapComponent;


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out ICombatTaker _) && other.gameObject.IsInLayerMask(targetLayer))
            {
                trapComponent.Complete();
            }
        }

        private void Awake()
        {
            TryGetComponent(out trapComponent);

            targetLayer            =   trapComponent.TargetLayer;
            triggerCollider        ??= GetComponent<SphereCollider>();
            triggerCollider.radius =   trapComponent.Radius;
            
            trapComponent.OnActivated.Register("TriggerOn", () => triggerCollider.enabled  = true);
            trapComponent.OnCompleted.Register("TriggerOff", () => triggerCollider.enabled = false);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            TryGetComponent(out triggerCollider);
        }
#endif
    }
}
