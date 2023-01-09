using Core;
using UnityEngine;

namespace Character.Combat.Entities
{
    public abstract class BaseEntity : MonoBehaviour, IActionSender
    {
        [SerializeField] protected EntityType flag;

        private int instanceID;

        public DataIndex ActionCode { get; protected set; }
        public EntityType Flag { get => flag; protected set => flag = value; }
        public ICombatProvider Provider { get; protected set; }
        
        public ActionTable OnStarted { get; } = new();
        public ActionTable OnCompleted { get; } = new();
        public ActionTable OnInterrupted { get; } = new();
        
        public abstract bool IsReady { get; }

        public int InstanceID
        {
            get
            {
                if (instanceID == 0)
                    instanceID = GetInstanceID();
                
                return instanceID;
            }
        }

        public virtual void Initialize(IActionSender actionSender)
        {
            ActionCode = actionSender.ActionCode;
            Provider = actionSender.Provider;
        }
    }
}
