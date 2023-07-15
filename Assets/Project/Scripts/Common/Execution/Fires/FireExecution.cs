using UnityEngine;

namespace Common.Execution.Fires
{
    public abstract class FireExecution : MonoBehaviour
    {
        [SerializeField] protected ExecuteGroup group = ExecuteGroup.Main;
        
        // TODO. 모든 CombatObject의 Executor가 HitExecutor, FireExecutor로 전환되면 private 삭제 후, Initialize는 protected Sender에 연결
        // private IActionSender sender;
        protected IActionSender Sender; 
            // => sender ??= GetComponentInParent<IActionSender>();
        
        public ExecuteGroup Group => group;

        public abstract void Fire(Vector3 position);

        public void Initialize(IActionSender sender)
        {
            // this.sender 
            Sender = sender;
        }
    }
}
