using Common.Traps;

namespace Character.Adventurers.Hunter.Trap
{
    public class OverTimeTrapSample : TrapComponent
    {
        public override void Execution()
        {
            if (TryGetTakerInSphere(out var takerList))
            {
                takerList.ForEach(ExecutionTable.Execute);
            }
        }
    }
}
