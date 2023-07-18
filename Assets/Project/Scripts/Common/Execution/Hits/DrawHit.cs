using UnityEngine;

namespace Common.Execution.Hits
{
    public class DrawHit : HitExecution
    {
        [SerializeField] private float drawDuration = 0.3f;
        
        public override void Hit(ICombatTaker taker)
        {
            if (taker?.DrawBehaviour is null) return;
            
            taker?.DrawBehaviour.Draw(transform.position, drawDuration);
        }
    }
}
