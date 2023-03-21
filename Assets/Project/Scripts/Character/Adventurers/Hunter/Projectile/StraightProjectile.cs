using Common;
using Common.Projectiles;

namespace Character.Adventurers.Hunter.Projectile
{
    public class StraightProjectile : ProjectileComponent
    {
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);
            
            OnCompleted.Register("Execution", Execution);
        }

        public override void Execution()
        {
            if (!TryGetTakerInSphere(out var takerList)) return;
            
            takerList.ForEach(ExecutionTable.Execute);
        }

        public override void Complete()
        {
            OnCompleted.Invoke();

            // End();
        }
    }
}
