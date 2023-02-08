using Core;
using UnityEngine;

namespace Character.Combat
{
    public class DamageModule : CombatModule, ICombatTable
    {
        [SerializeField] private PowerValue powerValue;
        
        public StatTable StatTable { get; } = new();
        public ICombatProvider Provider { get; set; }
        
        private DataIndex ActionCode { get; set; }

        private void OnActivated()
        {
            StatTable.Clear();
            StatTable.Register(ActionCode, powerValue);
            StatTable.UnionWith(Provider.StatTable);
        }
        
        public override void Initialize(CombatComponent combatComponent)
        {
            Provider   = combatComponent.Provider;
            ActionCode = combatComponent.ActionCode;
            
            combatComponent.OnActivated.Register(ActionCode.ToString(), OnActivated);
        }
        
        
#if UNITY_EDITOR
        public void SetUpValue(float value)
        {
            moduleType       = CombatModuleType.Damage;
            powerValue.Value = value;
        }
#endif
    }
}
