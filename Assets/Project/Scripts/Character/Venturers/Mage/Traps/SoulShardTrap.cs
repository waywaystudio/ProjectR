using Common;
using Common.Traps;
using UnityEngine;

namespace Character.Venturers.Mage.Traps
{
    public class SoulShardTrap : TrapComponent
    {
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            Builder.AddApplying("SpawnShard", SpawnShard);
        }


        private void SpawnShard(Vector3 position)
        {
            var randomVector2 = Random.insideUnitCircle * Radius;
            var tempDestination = new Vector3(position.x + randomVector2.x, 0f, position.z + randomVector2.y);

            PathfindingUtility.IsGround(tempDestination, out var groundPosition);

            var destination = new Vector3(groundPosition.x, groundPosition.y + 1f, groundPosition.z);

            transform.position = destination;
        }
    }
}
