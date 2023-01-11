using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Character.Combat
{
    public abstract class CombatObject : MonoBehaviour, IActionSender
    {
        [SerializeField] protected DataIndex actionCode;
        
        private int instanceID;
        protected int InstanceID =>
            instanceID == 0
                ? instanceID = GetInstanceID()
                : instanceID;

        public DataIndex ActionCode => actionCode;
        public virtual ICombatProvider Provider { get; set; }
        
        protected Dictionary<ModuleType, Module> ModuleTable { get; } = new();

        protected T GetModule<T>(ModuleType type) where T : Module =>
            ModuleTable.ContainsKey(type)
                ? ModuleTable[type] as T
                : null;

        public abstract void Initialize(ICombatProvider provider, ICombatTaker taker);

        protected virtual void Awake()
        {   
            GetComponents<Module>().ForEach(x => ModuleTable.Add(x.Flag, x));
        }
    }
}
