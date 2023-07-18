using UnityEngine;

namespace Common.Execution.Hits
{
    public class StunHit : HitExecution
    {
        [SerializeField] private float stunDuration;
        
        public override void Hit(ICombatTaker taker)
        {
            taker?.StunBehaviour.Stun(stunDuration);
        }
    }
}
