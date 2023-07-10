using Common.Execution.Fires;
using UnityEngine;

namespace Character.Venturers.Mage.Executions
{
    public class SoulShardFire : TrapFire
    {
        [SerializeField] private float probability = 1.0f;

        public override void Fire(Vector3 position)
        {
            if (probability == 0.0f) return;
            if (Random.value <= probability)
            {
                var soulShard = pool.Get();
                
                /* trap.Invoker.Active 실행 전에 Position을 옮겨주어야 함. */
                Transform transformRef;
                
                (transformRef = soulShard.transform).SetParent(null);
                transformRef.position = position;
                /**/
            
                soulShard.Invoker.Active(position);
            }
        }
    }
}
