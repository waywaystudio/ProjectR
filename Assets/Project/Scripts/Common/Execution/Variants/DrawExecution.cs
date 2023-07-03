using UnityEngine;

namespace Common.Execution.Variants
{
    public class DrawExecution : TakerExecution
    {
        [SerializeField] private float drawDuration = 0.3f;
        
        public override void Execution(ICombatTaker taker)
        {
            taker?.Draw(transform.position, drawDuration);
        }
    }
}
