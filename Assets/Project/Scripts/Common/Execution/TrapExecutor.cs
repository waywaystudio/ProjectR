using Common.Traps;
using UnityEngine;

namespace Common.Execution
{
    public class TrapExecutor : ExecuteComponent
    {
        [SerializeField] protected DataIndex actionCode;
        [SerializeField] private Pool<TrapComponent> pool;
        

        public override void Execution(Vector3 position)
        {
            pool.Get().Activate(position);
        }
        
        public override void Execution(ICombatTaker taker)
        {
            pool.Get().Activate(taker.Position);
        }
        

        private void CreateTrap(TrapComponent trap)
        {
            trap.Initialize(Origin.Provider);
            trap.SequenceBuilder.AddEnd("ReturnToPool",() =>
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
