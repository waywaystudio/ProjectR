using Core;
using UnityEngine;

namespace Character.Combat.Entities
{
    public abstract class BaseEntity : MonoBehaviour, IActionSender
    {
        [SerializeField] protected EntityType flag;

        private int instanceID;

        public IDCode ActionCode { get; private set; }
        public ICombatProvider Provider { get; protected set; }
        public EntityType Flag { get => flag; set => flag = value; }
        
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
