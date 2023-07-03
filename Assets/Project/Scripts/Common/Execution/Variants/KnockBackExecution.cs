using UnityEngine;

namespace Common.Execution.Variants
{
    public class KnockBackExecution : TakerExecution
    {
        [SerializeField] private float knockBackDistance = 8f;
        [SerializeField] private float knockBackDuration = 0.3f;
        
        public override void Execution(ICombatTaker taker)
        {
            taker?.KnockBack(transform.position, knockBackDistance, knockBackDuration);
        }
    }
}
