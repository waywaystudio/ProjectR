using Core;
using UnityEngine;

namespace Character.Combat
{
    public class HealModule : Module, IHealModule, ICombatTable
    {
        [SerializeField] private PowerValue healValue;

        public PowerValue HealValue => healValue;
        public StatTable StatTable { get; } = new();
        
        public override void Initialize(IActionSender actionSender)
        {
            base.Initialize(actionSender);
            
            // TODO. 현재 HealValue는 1.4, 2, 이런식이다. 값 알맞게 고쳐주거나, StatTable 함수에 곱 기능 추가해야 한다.
            StatTable.Register(ActionCode, HealValue);
            StatTable.UnionWith(Provider.StatTable);
        }


#if UNITY_EDITOR
        public void SetUpValue(float healValue)
        {
            Flag            = ModuleType.Heal;
            HealValue.Value = healValue;
        }
#endif
    }
}
