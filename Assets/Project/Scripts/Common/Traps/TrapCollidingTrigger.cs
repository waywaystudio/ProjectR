using UnityEngine;

namespace Common.Traps
{
    [RequireComponent(typeof(SphereCollider))]
    public class TrapCollidingTrigger : MonoBehaviour, IEditable
    {
        [SerializeField] protected SphereCollider triggerCollider;
        
        private LayerMask targetLayer;
        private Trap trap;


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out ICombatTaker _) && other.gameObject.IsInLayerMask(targetLayer))
            {
                trap.Invoker.Complete();
            }
        }

        private void Awake()
        {
            TryGetComponent(out trap);

            targetLayer            =   trap.TargetLayer;
            triggerCollider        ??= GetComponent<SphereCollider>();
            triggerCollider.radius =   trap.Radius;

            // Require Builder
            trap.Builder
                         .Add(Section.Active,"CollidingTriggerOn",
                              () => triggerCollider.IsNullOrDestroyed().OnFalse(() => triggerCollider.enabled = true))
                         .Add(Section.End,"CollidingTriggerOff", 
                              () => triggerCollider.IsNullOrDestroyed().OnFalse(() => triggerCollider.enabled = false));
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            TryGetComponent(out triggerCollider);
        }
#endif
    }
}
