using UnityEngine;

namespace Common.Execution
{
    public abstract class FireExecution : MonoBehaviour
    {
        [SerializeField] protected ExecuteGroup group = ExecuteGroup.Group1;
        
        private IOriginalProvider origin;
        
        public ExecuteGroup Group => group;
        protected IOriginalProvider Origin => origin ??= GetComponentInParent<IOriginalProvider>();
        
        public abstract void Execution(Vector3 position);
    }
}
