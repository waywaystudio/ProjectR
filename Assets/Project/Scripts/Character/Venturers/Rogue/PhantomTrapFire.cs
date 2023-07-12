using Common.Execution.Fires;
using UnityEngine;

namespace Character.Venturers.Rogue
{
    public class PhantomTrapFire : FireExecution
    {
        [SerializeField] private DataIndex actionCode;
        [SerializeField] private PhantomMaster master;
        [SerializeField] private Pool<PhantomTrap> pool;
        

        public override void Fire(Vector3 position)
        {
            var phantom = pool.Get();
            
            /* trap.Invoker.Active 실행 전에 Position을 옮겨주어야 함. */
            Transform transformRef;
                
            (transformRef = phantom.transform).SetParent(null);
            transformRef.position = position;
            /**/
            
            phantom.Invoker.Active(position);
        }


        private void CreateTrap(PhantomTrap trap)
        {
            trap.Initialize(Sender.Provider);
            trap.Builder
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
            pool.Initialize(CreateTrap, transform);
        }

        private void OnDisable()
        {
            master.Clear();
        }
    }
}