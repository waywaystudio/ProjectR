using Common;
using Common.Execution;
using Common.Execution.Variants;
using UnityEngine;

namespace Character.Venturers.Mage.Executions
{
    public class SoulShardExecution : TrapExecution
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

        // public override void Execution(ICombatTaker taker)
        // {
        //     if (probability == 0.0f) return;
        //     if (taker       == null) return;
        //     
        //     if (Random.value <= probability)
        //     {
        //         pool.Get().Activate(taker.Position);
        //     }
        // }
    }
}
