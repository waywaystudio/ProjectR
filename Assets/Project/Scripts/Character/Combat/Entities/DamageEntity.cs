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


#if UNITY_EDITOR
        public void SetUpValue(float damageValue)
        {
            DamageValue.Value = damageValue;
            Flag              = EntityType.Damage;
        }
#endif
    }
}