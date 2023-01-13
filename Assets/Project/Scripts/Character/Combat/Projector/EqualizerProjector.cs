using Core;

namespace Character.Combat.Projector
{
    public class EqualizerProjector : ProjectorObject
    {
        public void Test()
        {
            Projection(TempProvider, TempTarget);
        }


        protected override void EnterProjector(ICombatTaker taker)
        {
            // throw new System.NotImplementedException();
        }

        protected override void EndProjector(ICombatTaker taker)
        {
            if (DamageModule) taker.TakeDamage(DamageModule);
        }
    }
}
