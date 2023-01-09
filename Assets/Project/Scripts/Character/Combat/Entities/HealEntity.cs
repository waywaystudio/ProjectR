using Core;
using UnityEngine;

namespace Character.Combat.Entities
{
    public class HealEntity : BaseEntity, ICombatEntity
    {
        [SerializeField] private PowerValue healValue;

        public IDynamicStatEntry DynamicStatEntry => Provider.DynamicStatEntry;
        public StatTable StatTable { get; } = new();

        public override bool IsReady => true;
        public PowerValue HealValue { get => healValue; set => healValue = value; }

        private void Start()
        {
            // TODO. 현재 HealValue는 1.4, 2, 이런식이다. 값 알맞게 고쳐주거나, StatTable 함수에 곱 기능 추가해야 한다.
            StatTable.Register(ActionCode, HealValue);
            StatTable.UnionWith(Provider.StatTable);
        }
        
        
#if UNITY_EDITOR
        public void SetUpValue(float healValue)
        {
            HealValue.Value = healValue;
            Flag            = EntityType.Heal;
        }
#endif
    }
}
