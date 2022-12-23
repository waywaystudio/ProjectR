using Core;
using UnityEngine;
using AssignedSkillData = MainGame.Data.ContentData.SkillData.Skill;

namespace Common.Character.Operation.Combat
{
    public abstract class BaseEntity : MonoBehaviour
    {
        [SerializeField] protected EntityType flag;

        public string ActionName { get; protected set; }
        public ActionTable OnStarted { get; } = new();
        public ActionTable OnInterrupted { get; } = new();
        public ActionTable OnCompleted { get; } = new();

        public int InstanceID { get; private set; }
        public EntityType Flag => flag;
        public ICombatProvider Sender { get; protected set; }
        public abstract bool IsReady { get; }
        public abstract void SetEntity();

        protected AssignedSkillData Data
        {
            get
            {
                var assignedSkillName = GetComponent<IActionSender>().ActionName;
                return MainGame.MainData.GetSkillData(assignedSkillName);
            }
        }

        public void Initialize(IActionSender actionSender)
        {
            Sender = actionSender.Sender;
            ActionName = actionSender.ActionName;
        }
        
        protected virtual void Awake()
        {
            InstanceID = GetInstanceID();
            SetEntity();
        }
    }
}
