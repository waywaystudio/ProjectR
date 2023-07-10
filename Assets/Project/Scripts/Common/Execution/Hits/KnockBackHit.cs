using UnityEngine;

namespace Common.Execution.Hits
{
    public class KnockBackHit : HitExecution
    {
        [SerializeField] private float knockBackDistance = 8f;
        [SerializeField] private float knockBackDuration = 0.3f;
        
        public override void Hit(ICombatTaker taker)
        {
            taker?.KnockBack(transform.position, knockBackDistance, knockBackDuration);
        }
    }
}
