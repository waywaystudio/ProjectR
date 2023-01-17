using Core;
using UnityEngine;

namespace Character.Combat
{
    public class HealModule : CombatModule, ICombatTable
    {
        [SerializeField] private PowerValue healValue;

        public StatTable StatTable { get; } = new();
        
        // TODO. UnionWith가 Provider.StatTable 값 변동에 어떻게 대응하는지 확인이 필요하다.
        // TODO. 만약 유동적으로 대응한다면, Awake에서 한 번만 넣어주어도 된다.
        private void OnActivated()
        {
            StatTable.Clear();
            StatTable.Register(ActionCode, healValue);
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
            Flag            = ModuleType.Heal;
            healValue.Value = value;
        }
#endif
    }
}
