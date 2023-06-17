using UnityEngine;

namespace Common.Execution
{
    public class KnockBackExecutor : ExecuteComponent
    {
        [SerializeField] private float knockBackDistance = 8f;
        [SerializeField] private float knockBackDuration = 0.3f;
        
        public override void Execution(ICombatTaker taker)
        {
            taker.KnockBack(transform.position, knockBackDistance, knockBackDuration);
        }
    }
}
