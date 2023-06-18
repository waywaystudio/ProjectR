using Common;
using Common.Traps;

namespace Character.Venturers.Ranger.Trap
{
    public class OverTimeTrapSample : TrapComponent
    {
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            SequenceBuilder.Add(SectionType.Execute, "OverTimeTrapExecution", () =>
            {
                if (TryGetTakerInSphere(out var takerList))
                {
                    takerList.ForEach(executor.Execute);
                }
            });
        }

        public override void Execution() => SequenceInvoker.Execute();
    }
}
