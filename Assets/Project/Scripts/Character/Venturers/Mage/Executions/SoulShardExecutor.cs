using Common;
using Common.Execution;
using UnityEngine;

namespace Character.Venturers.Mage.Executions
{
    public class SoulShardExecutor : TrapExecutor
    {
        [SerializeField] private float probability = 1.0f;

        public override void Execution(Vector3 position)
        {
            if (probability == 0.0f) return;
            if (Random.value <= probability)
            {
                pool.Get().Activate(position);
            }
        }

        public override void Execution(ICombatTaker taker)
        {
            if (probability == 0.0f) return;
            if (taker       == null) return;
            
            if (Random.value <= probability)
            {
                pool.Get().Activate(taker.Position);
            }
        }
    }
}
