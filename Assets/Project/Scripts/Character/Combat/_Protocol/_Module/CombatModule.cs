using UnityEngine;

namespace Character.Combat
{
    public abstract class CombatModule : MonoBehaviour
    {
        [SerializeField] protected CombatModuleType moduleType;

        public CombatModuleType ModuleType => moduleType;
        
        public abstract void Initialize(CombatComponent combatComponent);
    }
}
