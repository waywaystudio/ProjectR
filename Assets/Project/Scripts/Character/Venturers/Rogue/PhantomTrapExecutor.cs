using Common;
using Common.Execution;
using UnityEngine;

namespace Character.Venturers.Rogue
{
    public class PhantomTrapExecutor : FireExecution
    {
        [SerializeField] private DataIndex actionCode;
        [SerializeField] private PhantomMaster master;
        [SerializeField] private Pool<PhantomTrap> pool;
        

        public override void Execution(Vector3 position)
        {
            pool.Get().Activate(position);
        }
        
        // public override void Execution(ICombatTaker taker)
        // {
        //     if (taker == null) return;
        //     
        //     pool.Get().Activate(taker.Position);
        // }
        

        private void CreateTrap(PhantomTrap trap)
        {
            trap.Initialize(Origin.Provider);
            trap.SequenceBuilder
                .Add(Section.Active, "AddMaster", () => master.Add(trap))
                .Add(Section.End,"ReturnToPool",() =>
                {
                    trap.transform.position = Vector3.zero;
                    trap.transform.SetParent(transform, false);
                    
                    master.Remove(trap);
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
            master.Clear();
        }
    }
}