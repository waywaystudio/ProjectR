using Core;
using UnityEngine;

namespace Character.Combat.Entities
{
    public class DamageEntity : BaseEntity, ICombatEntity
    {
        [SerializeField] private PowerValue damageValue;

        public IDynamicStatEntry DynamicStatEntry => Provider.DynamicStatEntry;
        public StatTable StatTable { get; } = new();
        public override bool IsReady => true;
        
        public PowerValue DamageValue { get => damageValue; set => damageValue = value; }

        private void Start()
        {
            StatTable.Register(ActionCode, DamageValue);
            StatTable.UnionWith(Provider.StatTable);
        }

        private void OnDisable() => StatTable.Unregister(ActionCode, DamageValue);
    }
}

/*
 * Annotation
 * 모든 스킬은 어떠한 Entity를 통해서 반드시 ICombatProvider를 가진다.
 * 다만 인터페이스를 스킬 클래스에 직접구현하지 않는데, ICombatProvider가 여럿일 수 있기 때문이다.
 * 만약 기술 클래스에 직접적으로 인터페이스를 달면, 하나 뿐히 달지 못한다.
 */