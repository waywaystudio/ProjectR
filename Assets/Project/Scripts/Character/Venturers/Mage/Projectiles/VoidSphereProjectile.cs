using System.Collections.Generic;
using System.Threading;
using Character.Venturers.Mage.Traps;
using Common;
using Common.Execution.Hits;
using Common.Projectiles;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character.Venturers.Mage.Projectiles
{
    public class VoidSphereProjectile : Projectile
    {
        [SerializeField] private DamageHit damageExecutor;
        [SerializeField] private SphereCollider triggerCollider;
        [SerializeField] private LayerMask trapLayer;
        [SerializeField] private float shardCollectableRadius;
        [SerializeField] private float absorbPower = 50f;

        private CancellationTokenSource cts;
        private float originalPower;
        private readonly Collider[] colliderBuffers = new Collider[16];


        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            Builder
                .Add(Section.Active, "CollidingTriggerOn", () => triggerCollider.enabled = true)
                .Add(Section.Active, "FindSoulShard", () => FindSoulShard().Forget())
                .Add(Section.End, "CollidingTriggerOff", () => triggerCollider.enabled   = false)
                .Add(Section.End, "ResetPower", ResetPower)
                .Add(Section.End, "StopFinding", StopFinding);
            
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
                Invoker.Hit(taker);
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
            TargetUtility.GetTargetsInSphere<SoulShardTrap>(transform.position, trapLayer, shardCollectableRadius, colliderBuffers);
            
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
            
            soul.Invoker.Complete();
        }

        private void ResetPower()
        {
            damageExecutor.DamageSpec.Change(StatType.Power, originalPower);
        }
    }
}
