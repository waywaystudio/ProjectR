using Core;
using UnityEngine;

namespace Character.Combat
{
    public class DamageModule : Module, IDamageModule, ICombatTable
    {
        [SerializeField] private PowerValue damageValue;

        public PowerValue DamageValue => damageValue;
        public StatTable StatTable { get; } = new();

        public override void Initialize(IActionSender actionSender)
        {
            base.Initialize(actionSender);
            
            StatTable.Register(ActionCode, DamageValue);
            StatTable.UnionWith(Provider.StatTable);
        }


#if UNITY_EDITOR
        public void SetUpValue(float damageValue)
        {
            Flag              = ModuleType.Damage;
            DamageValue.Value = damageValue;
        }
#endif
    }
}