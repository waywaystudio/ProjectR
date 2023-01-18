using Core;
using UnityEngine;

namespace Character.Combat
{
    public class DamageModule : CombatModule, ICombatTable
    {
        [SerializeField] private PowerValue damageValue;

        public StatTable StatTable { get; } = new();
        public ICombatProvider Provider => CombatObject.Provider;
        public DataIndex ActionCode => CombatObject.ActionCode;

        private void OnActivated()
        {
            StatTable.Clear();
            StatTable.Register(ActionCode, damageValue);
            StatTable.UnionWith(Provider.StatTable);
        }

        protected override void Awake()
        {
            base.Awake();
            
            CombatObject.OnActivated.Register(InstanceID, OnActivated);
        }


#if UNITY_EDITOR
        public void SetUpValue(float value)
        {
            Flag              = ModuleType.Damage;
            damageValue.Value = value;
        }
#endif
    }
}