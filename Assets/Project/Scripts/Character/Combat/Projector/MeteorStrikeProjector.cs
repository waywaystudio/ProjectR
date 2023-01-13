using Core;

namespace Character.Combat.Projector
{
    public class MeteorStrikeProjector : ProjectorObject
    {
        protected override void EnterProjector(ICombatTaker taker)
        {
            /* OnCollisionEntered :: Avoid */
            // Try use Method 4: The ConstantPath type
            // https://arongranberg.com/astar/documentation/dev_4_3_61_b7b7a3f3/wander.html
        }

        protected override void EndProjector(ICombatTaker taker)
        {
            if (DamageModule) taker.TakeDamage(DamageModule);
        }
    }
}

