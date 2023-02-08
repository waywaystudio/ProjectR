using Character.Combat.Skill;
using Core;
using UnityEngine;

namespace Character.Combat
{
    public class OldCombatModule : MonoBehaviour
    {
        public CombatObject CombatObject;
        public CombatModuleType Flag;

        private int instanceID;

        protected int InstanceID =>
            instanceID == 0
                ? instanceID = GetInstanceID()
                : instanceID;

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
