using Character.Combat.Skill;
using Core;
using UnityEngine;

namespace Character.Combat
{
    public class CombatModule : MonoBehaviour
    {
        public CombatObject CombatObject;
        public ModuleType Flag;

        private int instanceID;

        protected int InstanceID =>
            instanceID == 0
                ? instanceID = GetInstanceID()
                : instanceID;


        public virtual void Initialize(ICombatProvider provider, IActionSequence actionSequence) { }

        protected virtual void Awake()
        {
            CombatObject = GetComponent<CombatObject>();
            CombatObject.ModuleTable.TryAdd(Flag, this);
        }

        public void SetUp()
        {
            CombatObject = GetComponent<CombatObject>();
        }
    }
}
