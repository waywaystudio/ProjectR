using Common.Traps;
using UnityEngine;

namespace Common.Execution.Variants
{
    public class TrapExecution : FireExecution
    {
        [SerializeField] protected Pool<Trap> pool;

        public override void Fire(Vector3 position)
        {
            var trap = pool.Get();
            
            trap.Invoker.Active(position);
        }


        private void CreateTrap(Trap trap)
        {
            trap.Initialize(Sender.Provider);
            trap.Builder.Add(Section.End,"ReturnToPool",() => ReturnToPool(trap));
        }

        private void ReturnToPool(Trap trap)
        {
            trap.transform.position = Vector3.zero;
            trap.transform.SetParent(transform, false);
                    
            pool.Release(trap);
        }

        private void OnEnable()
        {
            pool.Initialize(CreateTrap, transform,
                null,
                null,
                trap => trap.Dispose());
        }

        private void OnDisable()
        {
            pool.Clear();
        }
    }
}
