using Common.Execution.Variants;
using UnityEngine;

namespace Character.Venturers.Mage.Executions
{
    public class SoulShardExecution : TrapExecution
    {
        [SerializeField] private float probability = 1.0f;

        public override void Fire(Vector3 position)
        {
            if (probability == 0.0f) return;
            if (Random.value <= probability)
            {
                var soulShard = pool.Get();
            
                soulShard.Invoker.Active(position);
            }
        }
    }
}
