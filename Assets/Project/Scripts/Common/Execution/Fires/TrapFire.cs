using Common.Traps;
using UnityEngine;

namespace Common.Execution.Fires
{
    public class TrapFire : FireExecution
    {
        [SerializeField] protected Pool<Trap> pool;

        public override void Fire(Vector3 position)
        {
            var trap = pool.Get();
            
            /* trap.Invoker.Active 실행 전에 Position을 옮겨주어야 함. */
            Transform transformRef;
            
            (transformRef = trap.transform).SetParent(null);
            transformRef.position = position;
            /**/

            trap.Invoker.Active(position);
        }


        private void CreateTrap(Trap trap)
        {
            trap.Initialize(Sender.Provider);
            trap.Builder
                .Add(Section.End,"ReturnToPool",() => ReturnToPool(trap));
        }

        private void ReturnToPool(Trap trap)
        {
            trap.transform.position = Vector3.zero;
            trap.transform.SetParent(transform, false);
                    
            pool.Release(trap);
        }

        private void OnEnable()
        {
            pool.Initialize(CreateTrap, transform);
        }
    }
}
