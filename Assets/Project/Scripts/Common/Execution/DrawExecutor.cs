using UnityEngine;

namespace Common.Execution
{
    public class DrawExecutor : ExecuteComponent
    {
        [SerializeField] private float drawDuration = 0.3f;
        
        public override void Execution(ICombatTaker taker)
        {
            taker?.Draw(transform.position, drawDuration);
        }
    }
}
