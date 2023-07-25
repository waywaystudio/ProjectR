using UnityEngine;

namespace Common.Execution.Hits
{
    public abstract class HitExecution : MonoBehaviour
    {
        [SerializeField] protected ExecuteGroup group = ExecuteGroup.Main;

        protected IActionSender Sender;
        
        public ExecuteGroup Group => group;
        
        public abstract void Hit(ICombatTaker taker);

        public void Initialize(IActionSender sender)
        {
            // this.sender 
            Sender = sender;
        }
    }
}
