using Core;
using UnityEngine;

namespace Character.Data
{
    public class DynamicStatEntry : MonoBehaviour, IDynamicStatEntry
    {
        private CharacterBehaviour cb;
        private int instanceID;

        public AliveValue IsAlive { get; } = new();
        public HpValue Hp { get; } = new();
        public ResourceValue Resource { get; } = new();
        public ShieldValue Shield { get; } = new();
        public StatusEffectTable BuffTable { get; } = new();
        public StatusEffectTable DeBuffTable { get; } = new();
        

        private void Register(IStatusEffect statusEffect)
        {
            var targetTable = statusEffect.IsBuff
                ? BuffTable
                : DeBuffTable;
            
            targetTable.Register(statusEffect);

            statusEffect.TargetTable = targetTable;
        }

        private void Awake()
        {
            cb                  = GetComponentInParent<CharacterBehaviour>();
            cb.DynamicStatEntry = this;
            Hp.StatTable        = cb.StatTable;
            Resource.StatTable  = cb.StatTable;
            Shield.StatTable    = cb.StatTable;
            instanceID          = GetInstanceID();
            
            cb.OnTakeStatusEffect.Register(instanceID, Register);
        }

        private void Start()
        {
            IsAlive.Value  = true;
            Hp.Value       = cb.StatTable.MaxHp;
            Resource.Value = cb.StatTable.MaxResource;
            Shield.Value   = 0;
        }
    }
}
