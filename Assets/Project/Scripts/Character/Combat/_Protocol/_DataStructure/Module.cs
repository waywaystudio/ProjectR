using Core;
using UnityEngine;

namespace Character.Combat
{
    public class Module : MonoBehaviour, IModule
    {
        [SerializeField] private ModuleType flag;

        private int instanceID;
        
        public DataIndex ActionCode { get; protected set; }
        public ICombatProvider Provider { get; protected set; }
        public ModuleType Flag { get => flag; protected set => flag = value; }
        public int InstanceID =>
            instanceID == 0
                ? instanceID = GetInstanceID()
                : instanceID;

        public virtual void Initialize(IActionSender actionSender)
        {
            ActionCode = actionSender.ActionCode;
            Provider   = actionSender.Provider;
        }
    }
}
