using UnityEngine;

namespace Common.Execution.Variants
{
    public class StunExecution : TakerExecution
    {
        [SerializeField] private float stunDuration;
        
        public override void Execution(ICombatTaker taker)
        {
            taker?.Stun(stunDuration);
        }
    }
}
