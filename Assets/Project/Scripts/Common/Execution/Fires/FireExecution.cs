using UnityEngine;

namespace Common.Execution.Fires
{
    public abstract class FireExecution : MonoBehaviour
    {
        [SerializeField] protected ExecuteGroup group = ExecuteGroup.Main;

        protected IActionSender Sender; 
        
        public ExecuteGroup Group => group;

        public abstract void Fire(Vector3 position);

        public void Initialize(IActionSender sender)
        {
            // this.sender 
            Sender = sender;
        }
    }
}
