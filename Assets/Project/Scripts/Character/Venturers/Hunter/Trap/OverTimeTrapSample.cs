using Common.Traps;

namespace Character.Venturers.Hunter.Trap
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
