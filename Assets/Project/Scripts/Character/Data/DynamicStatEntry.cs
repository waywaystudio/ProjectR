using Core;
using UnityEngine;

namespace Character.Data
{
    public class DynamicStatEntry : MonoBehaviour, IDynamicStatEntry
    {
        public AliveValue IsAlive { get; } = new();
        public HpValue Hp { get; } = new();
        public ResourceValue Resource { get; } = new();
        public ShieldValue Shield { get; } = new();
        public StatusEffectTable BuffTable { get; } = new();
        public StatusEffectTable DeBuffTable { get; } = new();


        public void Initialize(StatTable statTable)
        {
            Hp.StatTable       = statTable;
            Resource.StatTable = statTable;
            Shield.StatTable   = statTable;
            
            IsAlive.Value  = true;
            Hp.Value       = statTable.MaxHp;
            Resource.Value = statTable.MaxResource;
            Shield.Value   = 0;
        }

        public void Register(IStatusEffect statusEffect)
        {
            var targetTable = statusEffect.IsBuff
                ? BuffTable
                : DeBuffTable;
            
            targetTable.Register(statusEffect);

            statusEffect.TargetTable = targetTable;
        }
    }
}
