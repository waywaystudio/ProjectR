using System.Collections.Generic;
using Common;
using Common.Projectiles;
using UnityEngine;

namespace Character.Venturers.Ranger.Projectile
{
    [RequireComponent(typeof(SphereCollider))]
    public class ExplodeProjectile : ProjectileComponent
    {
        [SerializeField] private SphereCollider triggerCollider;
        [SerializeField] private float radius;
        
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);
            
            sequencer.ActiveAction.Add("CollidingTriggerOn", () => triggerCollider.enabled = true);
            sequencer.CompleteAction.Add("ExplodeExecution", Execution);
            sequencer.EndAction.Add("CollidingTriggerOff", () => triggerCollider.enabled = false);
        }

        public override void Execution()
        {
            if (!TryGetTakerInSphere(out var takerList)) return;

            takerList.ForEach(executor.Execute);
        }


        private bool TryGetTakerInSphere(out List<ICombatTaker> takerList)
            => collidingSystem.TryGetTakersInSphere(transform.position, 
                radius, 
                360f, 
                targetLayer, 
                out takerList);

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out ICombatTaker _) && other.gameObject.IsInLayerMask(targetLayer))
            {
                sequencer.Complete();
            }
        }
    }
}
