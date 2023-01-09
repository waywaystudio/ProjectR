using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Character.Data
{
    public class DynamicStatEntry : MonoBehaviour, IDynamicStatEntry
    {
        private CharacterBehaviour cb;
        private int instanceID;

        [ShowInInspector] public AliveValue IsAlive { get; } = new();
        [ShowInInspector] public HpValue Hp { get; } = new();
        [ShowInInspector] public ResourceValue Resource { get; } = new();
        [ShowInInspector] public ShieldValue Shield { get; } = new();
        [ShowInInspector] public StatusEffectTable BuffTable { get; } = new();
        [ShowInInspector] public StatusEffectTable DeBuffTable { get; } = new();

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
            cb                 = GetComponentInParent<CharacterBehaviour>();
            Hp.StatTable       = cb.StatTable;
            Resource.StatTable = cb.StatTable;
            Shield.StatTable   = cb.StatTable;
            instanceID         = GetInstanceID();
        }

        private void OnEnable()
        {
            cb.DynamicStatEntry = this;
            cb.OnTakeStatusEffect.Register(instanceID, Register);
        }
        
        private void Start()
        {
            IsAlive.Value  = true;
            Hp.Value       = cb.StatTable.MaxHp;
            Resource.Value = cb.StatTable.MaxResource;
            Shield.Value   = 0;
        }

        // TODO. 유니티 Disable Destroy 순서미보장 문제로 인한 종료에러...
        // private void OnDisable()
        // {
        //     cb.DynamicStatEntry = null;
        //     cb.OnTakeStatusEffect.Unregister(instanceID);
        // }
    }
}
