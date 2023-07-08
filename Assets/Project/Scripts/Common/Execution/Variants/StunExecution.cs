using UnityEngine;

namespace Common.Execution.Variants
{
    public class StunExecution : HitExecution
    {
        [SerializeField] private float stunDuration;
        
        public override void Hit(ICombatTaker taker)
        {
            taker?.Stun(stunDuration);
        }
    }
}
