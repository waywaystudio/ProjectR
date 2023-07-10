using UnityEngine;

namespace Common.Execution.Hits
{
    public class DrawHit : HitExecution
    {
        [SerializeField] private float drawDuration = 0.3f;
        
        public override void Hit(ICombatTaker taker)
        {
            taker?.Draw(transform.position, drawDuration);
        }
    }
}
