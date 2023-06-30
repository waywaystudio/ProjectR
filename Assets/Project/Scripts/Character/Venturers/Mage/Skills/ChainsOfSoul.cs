using System;
using System.Collections.Generic;
using Character.Venturers.Mage.Traps;
using Common;
using Common.Skills;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Character.Venturers.Mage.Skills
{
    public class ChainsOfSoul : SkillComponent
    {
        [SerializeField] private LayerMask trapLayer;
        [SerializeField] private float shardCollectableRadius = 100f;
        [SerializeField] private float drawDuration = 1.0f;
        [SerializeField] private Ease drawEaseType;
        
        
        public override void Initialize()
        {
            base.Initialize();

            SequenceBuilder.AddActiveParam("CondenseSoulShard", CondenseSoulShard);
            // .Add(SectionType.Execute, "PlayEndChargingAnimation", PlayEndChargingAnimation)
            // .Add(SectionType.Execute, "SetIsActiveTrue", () => SkillInvoker.IsActive = false)
            // .Add(SectionType.End, "StopTracking", StopTracking);
        }

        private void CondenseSoulShard(Vector3 targetPosition)
        {
            var validPosition = ValidPosition(targetPosition);
            var collectableSoulShards = GetShardsInStage(validPosition);

            if (collectableSoulShards.IsNullOrEmpty()) return;
            
            collectableSoulShards.ForEach(soulShard =>
            {
                DrawShards(soulShard, validPosition, null);
            });
        }
        
        /// <summary>
        /// 플레이어가 선택한 마우스가 사거리 밖일 경우, 삼거리 내에서 가장 가까운 지점을 반환한다.
        /// </summary>
        private Vector3 ValidPosition(Vector3 targetPosition)
        {
            var playerPosition = Cb.transform.position;
            
            if (Vector3.Distance(playerPosition, targetPosition) <= Range)
            {
                return targetPosition;
            }

            var direction = (targetPosition - playerPosition).normalized;
            var destination = playerPosition + direction * Range;

            return destination;
        }
        
        private List<SoulShardTrap> GetShardsInStage(Vector3 targetPosition)
        {
            var colliderBuffers = new Collider[16];
            
            if (Physics.OverlapSphereNonAlloc(targetPosition, shardCollectableRadius, colliderBuffers, trapLayer) == 0) return null;
        
            var result = new List<SoulShardTrap>();
            
            colliderBuffers.ForEach(collider =>
            {
                if (collider is null) return;
                if (collider.IsNullOrEmpty() || !collider.TryGetComponent(out SoulShardTrap soulShard)) return;

                result.Add(soulShard);
            });
        
            return result;
        }

        private void DrawShards(SoulShardTrap soulShard, Vector3 dest, Action callback)
        {
            var shardsPosition = soulShard.gameObject.transform.position;
            var drawDirection = dest - shardsPosition;
            var drawDistance = Vector3.Distance(dest, shardsPosition) * Random.Range(0.8f, 1.2f);
            
            var drawDestination = PathfindingUtility.GetReachableStraightPosition(shardsPosition, drawDirection, drawDistance);
            
            soulShard.transform
                     .DOMove(drawDestination, drawDuration)
                     .SetEase(drawEaseType)
                     .OnComplete(() => callback?.Invoke());
        }
    }
}
