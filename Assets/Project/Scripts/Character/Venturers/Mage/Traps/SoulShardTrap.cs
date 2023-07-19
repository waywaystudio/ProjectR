using Common;
using Common.Traps;
using DG.Tweening;
using UnityEngine;

namespace Character.Venturers.Mage.Traps
{
    public class SoulShardTrap : Trap
    {
        private Tween spawnTween;
        
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            Builder
                .AddApplying("SpawnShard", SpawnShard)
                .Add(Section.Cancel, "SpawnCancel", SpawnCancel)
                .Add(Section.End, "SpawnCancel", SpawnCancel);
        }
        
        protected override void Dispose()
        {
            base.Dispose();

            SpawnCancel();
        }


        private void SpawnShard(Vector3 position)
        {
            var randomVector2 = Random.insideUnitCircle * Radius;
            var tempDestination = new Vector3(position.x + randomVector2.x, 0f, position.z + randomVector2.y);

            if (!PathfindingUtility.IsGround(tempDestination, out var groundPosition))
            {
                groundPosition = PathfindingUtility.GetNearestSafePosition(tempDestination, SizeEntity.AreaRange);
            }

            var destination = new Vector3(groundPosition.x, groundPosition.y + 1f, groundPosition.z);

            spawnTween = transform.DOJump(destination, 2.4f, 1, 0.33f);
        }

        private void SpawnCancel()
        {
            if (spawnTween == null) return;
            
            spawnTween.Kill();
            spawnTween = null;
        }
    }
}
