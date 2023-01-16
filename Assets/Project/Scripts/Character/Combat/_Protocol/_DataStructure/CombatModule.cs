using Core;
using UnityEngine;

namespace Character.Combat
{
    public class CombatModule : MonoBehaviour /*, ICombatModule */
    {
        [SerializeField] private ModuleType flag;

        private int instanceID;

        public ICombatObject CombatObject { get; protected set; }
        public DataIndex ActionCode => CombatObject.ActionCode;
        public ICombatProvider Provider => CombatObject.Provider;
        public ModuleType Flag { get => flag; protected set => flag = value; }
        
        protected int InstanceID =>
            instanceID == 0
                ? instanceID = GetInstanceID()
                : instanceID;

        public virtual void Initialize(ICombatObject combatObject) { }
        protected virtual void Awake()
        {
            CombatObject = GetComponent<ICombatObject>();
            CombatObject.ModuleTable.TryAdd(Flag, this);
        }
    }
}
