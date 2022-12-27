using Core;
using UnityEngine;

namespace Common.Character.Operation.Combat
{
    public abstract class BaseEntity : MonoBehaviour
    {
        [SerializeField] protected EntityType flag;

        public string ActionName { get; set; }
        public ActionTable OnStarted { get; } = new();
        public ActionTable OnCompleted { get; } = new();
        public ActionTable OnInterrupted { get; } = new();
        public EntityType Flag { get => flag; set => flag = value; }
        public ICombatProvider Sender { get; protected set; }
        public abstract bool IsReady { get; }
        protected int InstanceID { get; private set; }

        public void Initialize(IActionSender actionSender)
        {
            Sender = actionSender.Sender;
            ActionName = actionSender.ActionName;
        }
        
        protected virtual void Awake()
        {
            InstanceID = GetInstanceID();
        }
    }
}
