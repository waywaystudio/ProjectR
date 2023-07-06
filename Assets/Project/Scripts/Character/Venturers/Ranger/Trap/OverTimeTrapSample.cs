using Common;
using Common.Traps;

namespace Character.Venturers.Ranger.Trap
{
    public class OverTimeTrapSample : TrapComponent
    {
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            SequenceBuilder.Add(Section.Execute, "OverTimeTrapExecution", () =>
            {
                if (TryGetTakerInSphere(out var takerList))
                {
                    takerList.ForEach(executor.ToTaker);
                }
            });
        }

        public override void Execution() => SequenceInvoker.Execute();
    }
}
