using UnityEngine;

namespace Common.Execution
{
    public class StunExecutor : ExecuteComponent
    {
        [SerializeField] private float stunDuration;
        
        public override void Execution(ICombatTaker taker)
        {
            taker?.Stun(stunDuration);
        }
    }
}
