using Common;
using Common.Traps;

namespace Character.Venturers.Hunter.Trap
{
    public class ExplodeTrapSample : TrapComponent
    {
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);
            
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
