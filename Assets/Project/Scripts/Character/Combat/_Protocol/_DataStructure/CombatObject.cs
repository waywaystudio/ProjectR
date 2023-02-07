using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Character.Combat
{
    public abstract class CombatObject : MonoBehaviour, IInspectorSetUp
    {
        [SerializeField] protected DataIndex actionCode;
        
        private int instanceID;

        public DataIndex ActionCode => actionCode;
        public virtual ICombatProvider Provider { get; protected set; }
        
        public OldActionTable OnActivated { get; } = new();
        public OldActionTable OnCompleted { get; } = new();
        public OldActionTable OnCanceled { get; } = new();
        public OldActionTable OnHit { get; } = new();
        
        public List<IReady> ReadyCheckList { get; } = new();
        public Dictionary<ModuleType, CombatModule> ModuleTable { get; } = new();
        
        public DamageModule DamageModule => GetModule<DamageModule>(ModuleType.Damage);
        public CastingModule CastingModule => GetModule<CastingModule>(ModuleType.Casting);
        public CoolTimeModule CoolTimeModule => GetModule<CoolTimeModule>(ModuleType.CoolTime);
        public HealModule HealModule => GetModule<HealModule>(ModuleType.Heal);
        public ProjectileModule ProjectileModule => GetModule<ProjectileModule>(ModuleType.Projectile);
        public StatusEffectModule StatusEffectModule => GetModule<StatusEffectModule>(ModuleType.StatusEffect);
        public TargetModule TargetModule => GetModule<TargetModule>(ModuleType.Target);
        public ResourceModule ResourceModule => GetModule<ResourceModule>(ModuleType.Resource);
        public ProjectorModule ProjectorModule => GetModule<ProjectorModule>(ModuleType.Projector);
        
        public int InstanceID =>
            instanceID == 0
                ? instanceID = GetInstanceID()
                : instanceID;


        public virtual void Active() => OnActivated.Invoke();
        public virtual void Complete() => OnCompleted.Invoke();
        public virtual void Cancel() => OnCanceled.Invoke();
        public virtual void Hit() => OnHit.Invoke();
        
        
        private T GetModule<T>(ModuleType type) where T : CombatModule =>
            ModuleTable.ContainsKey(type)
                ? ModuleTable[type] as T
                : null;


        public virtual void SetUp()
        {
            if (actionCode == DataIndex.None)
                actionCode = name.ToEnum<DataIndex>();
        }
    }
}
