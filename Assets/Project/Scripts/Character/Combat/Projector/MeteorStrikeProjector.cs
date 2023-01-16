using Core;

namespace Character.Combat.Projector
{
    public class MeteorStrikeProjector : ProjectorObject
    {
        protected override void EnterProjector(ICombatTaker taker)
        {
            // taker.Survival();
            /* --------------------- */
            
            // cb.BehaviourStatus = BehaviourStatus.Survival;
            // if (cb.casting) cancel skill;
            
            // +callback cb.BehaviourStatus = BehaviourStatus.Combat;

        }

        protected override void EndProjector(ICombatTaker taker)
        {
            if (DamageModule) taker.TakeDamage(DamageModule);
        }
    }
}

