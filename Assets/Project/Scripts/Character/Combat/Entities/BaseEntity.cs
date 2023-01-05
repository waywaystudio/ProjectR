using Core;
using UnityEngine;

namespace Character.Combat.Entities
{
    public abstract class BaseEntity : MonoBehaviour
    {
        [SerializeField] protected EntityType flag;

        private int instanceID;

        /* Inherit for ICombatProvider Entity == Assigned ActionCode (ex.CommonAttack) */
        public IDCode ActionCode { get; private set; }
        public ICombatProvider Provider { get; protected set; }
        
        public ActionTable OnStarted { get; } = new();
        public ActionTable OnCompleted { get; } = new();
        public ActionTable OnInterrupted { get; } = new();
        public EntityType Flag { get => flag; set => flag = value; }
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
            Provider = actionSender.Provider;
            ActionCode = actionSender.ActionCode;
        }
    }
}
