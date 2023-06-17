using Common;
using Common.Traps;

namespace Character.Venturers.Ranger.Trap
{
    public class OverTimeTrapSample : TrapComponent
    {
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);
            
            ExecuteAction.Add("OverTimeTrapExecution", () =>
            {
                if (TryGetTakerInSphere(out var takerList))
                {
                    takerList.ForEach(executor.Execute);
                }
            });
        }

        public override void Execution() => ExecuteAction.Invoke();
    }
}
