using System.Collections.Generic;
using System.Threading;
using Character.Venturers.Mage.Traps;
using Common;
using Common.Execution;
using Common.Execution.Variants;
using Common.Projectiles;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character.Venturers.Mage.Projectiles
{
    public class VoidSphereProjectile : ProjectileComponent
    {
        [SerializeField] private DamageExecution damageExecutor;
        [SerializeField] private SphereCollider triggerCollider;
        [SerializeField] private LayerMask trapLayer;
        [SerializeField] private float shardCollectableRadius;
        [SerializeField] private float absorbPower = 50f;

        private CancellationTokenSource cts;
        private float originalPower;


        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            Builder
                .Add(SectionType.Active, "CollidingTriggerOn", () => triggerCollider.enabled = true)
                .Add(SectionType.Active, "FindSoulShard", () => FindSoulShard().Forget())
                .Add(SectionType.End, "CollidingTriggerOff", () => triggerCollider.enabled   = false)
                .Add(SectionType.End, "ResetPower", ResetPower)
                .Add(SectionType.End, "StopFinding", StopFinding);
            
            originalPower = damageExecutor.DamageSpec.Power;
        }
        
        
        protected override void Dispose()
        {
            base.Dispose();

            StopFinding();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out ICombatTaker taker) &&
                other.gameObject.IsInLayerMask(targetLayer) &&
                taker.Alive.Value)
            {
                executor.ToTaker(taker);
                Invoker.Complete();
            }
        }

        private async UniTaskVoid FindSoulShard()
        {
            cts = new CancellationTokenSource();

            while (true)
            {
                var soulShards = GetShardsInSphere();

                if (soulShards.HasElement())
                {
                    soulShards.ForEach(AbsorbSoul);
                }

                if (Invoker.IsEnd)
                {
                    StopFinding();
                    return;
                }
                
                await UniTask.Yield(PlayerLoopTiming.LastUpdate, cts.Token);
            }
        }

        private void StopFinding()
        {
            cts?.Cancel();
            cts = null;
        }
        
        private List<SoulShardTrap> GetShardsInSphere()
        {
            var colliderBuffers = new Collider[16];
            
            if (Physics.OverlapSphereNonAlloc(transform.position, shardCollectableRadius, colliderBuffers, trapLayer) == 0) return null;
        
            var result = new List<SoulShardTrap>();
            
            colliderBuffers.ForEach(collider =>
            {
                if (collider is null) return;
                if (collider.IsNullOrEmpty() || !collider.TryGetComponent(out SoulShardTrap soulShard)) return;

                result.Add(soulShard);
            });

            return result;
        }

        private void AbsorbSoul(SoulShardTrap soul)
        {
            if (!soul.isActiveAndEnabled) return;
            
            var currentPower = damageExecutor.DamageSpec.Power;
            var absorbedPower = currentPower + absorbPower;
            
            damageExecutor.DamageSpec.Change(StatType.Power, absorbedPower);
            
            soul.SequenceInvoker.Complete();
        }

        private void ResetPower()
        {
            damageExecutor.DamageSpec.Change(StatType.Power, originalPower);
        }
    }
}
