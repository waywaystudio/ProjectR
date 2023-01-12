using Core;

namespace Character.Combat.Projector
{
    public class MeteorStrikeProjector : ProjectorObject
    {
        public void Test()
        {
            Projection(TempProvider, TempTarget);
        }

        protected override void EnterProjector(ICombatTaker taker)
        {
            
        }

        protected override void EndProjector(ICombatTaker taker)
        {
            taker.TakeDamage(DamageModule);
        }

        protected override void Awake()
        {
            base.Awake();
            
            OnProjectorEnd.Register(InstanceID, EndProjector);
        }
    }
}

