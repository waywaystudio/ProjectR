using Common;
using Common.Traps;
using UnityEngine;

namespace Character.Adventurers.Hunter.Trap
{
    public class ExplodeTrapSample : TrapComponent
    {
        public override void Initialize(ICombatProvider provider, Vector3 position)
        {
            base.Initialize(provider, position);
            
            OnCompleted.Register("Execution", Execution);
        }
        
        public override void Execution()
        {
            if (TryGetTakerInSphere(out var takerList))
            {
                takerList.ForEach(ExecutionTable.Execute);
            }
        }
    }
}
