using Sirenix.OdinInspector;
using UnityEngine;

namespace Character.Data
{
    public class DynamicStatEntry : MonoBehaviour, IDynamicStatEntry
    {
        public AliveValue Alive { get; } = new();
        public HpValue Hp { get; } = new();
        public ResourceValue Resource { get; } = new();
        public ShieldValue Shield { get; } = new();
        [ShowInInspector]
        public StatTable StatTable { get; } = new();
        public StatusEffectTable BuffTable { get; } = new();
        public StatusEffectTable DeBuffTable { get; } = new();


        public void Initialize()
        {
            Hp.StatTable       = StatTable;
            Resource.StatTable = StatTable;
            Shield.StatTable   = StatTable;
            
            Alive.Value  = true;
            Hp.Value       = StatTable.MaxHp;
            Resource.Value = StatTable.MaxResource;
            Shield.Value   = 0;
        }
    }
}
