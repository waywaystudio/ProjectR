using Common.Traps;
using UnityEngine;

namespace Common.Execution.Variants
{
    public class TrapExecution : FireExecution
    {
        [SerializeField] protected Pool<TrapComponent> pool;

        public override void Execution(Vector3 position)
        {
            pool.Get().Activate(position);
        }


        private void CreateTrap(TrapComponent trap)
        {
            trap.Initialize(Origin.Provider);
            trap.SequenceBuilder.Add(Section.End,"ReturnToPool",() =>
            {
                trap.transform.position = Vector3.zero;
                trap.transform.SetParent(transform, false);
                    
                pool.Release(trap);
            });
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
