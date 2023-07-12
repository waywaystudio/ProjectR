using System;
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
        [SerializeField] private float drawDuration = 1.0f;
        [SerializeField] private Ease drawEaseType;
        
        private readonly Collider[] colliderBuffers = new Collider[64];
        private Tween drawTween;
        
        public override void Initialize()
        {
            base.Initialize();

            Builder
                .AddApplying("CondenseSoulShard", CondenseSoulShard)
                .Add(Section.End, "StopTween", StopTween);
        }
        
        
        protected override void Dispose()
        {
            base.Dispose();

            StopTween();
        }

        private void CondenseSoulShard(Vector3 targetPosition)
        {
            var validPosition = TargetUtility.GetValidPosition(Cb.transform.position, Range, targetPosition);
            var collectableSoulShards = TargetUtility.GetTargetsInSphere<SoulShardTrap>(
                targetPosition, 
                trapLayer, 
                Range, 
                colliderBuffers);

            if (collectableSoulShards.IsNullOrEmpty()) return;
            
            collectableSoulShards.ForEach(soulShard =>
            {
                DrawShards(soulShard, validPosition, null);
            });
        }

        private void DrawShards(SoulShardTrap soulShard, Vector3 dest, Action callback)
        {
            var shardsPosition = soulShard.gameObject.transform.position;
            var drawDirection = dest - shardsPosition;
            var drawDistance = Vector3.Distance(dest, shardsPosition) * Random.Range(0.8f, 1.2f);
            
            var drawDestination = PathfindingUtility.GetReachableStraightPosition(shardsPosition, drawDirection, drawDistance);
            
            drawTween = soulShard.transform
                                 .DOMove(drawDestination, drawDuration)
                                 .SetEase(drawEaseType)
                                 .OnComplete(() => callback?.Invoke());
        }

        private void StopTween()
        {
            if (drawTween == null) return;
            
            drawTween.Kill();
            drawTween = null;
        }
    }
}
