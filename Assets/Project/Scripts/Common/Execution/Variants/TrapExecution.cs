using Common.Traps;
using UnityEngine;

namespace Common.Execution.Variants
{
    public class TrapExecution : FireExecution
    {
        [SerializeField] protected Pool<TrapComponent> pool;

        public override void Fire(Vector3 position)
        {
            pool.Get().Activate(position);
        }


        private void CreateTrap(TrapComponent trap)
        {
            trap.Initialize(Sender.Provider);
            trap.Builder.Add(Section.End,"ReturnToPool",() => ReturnToPool(trap));
        }

        private void ReturnToPool(TrapComponent trap)
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
