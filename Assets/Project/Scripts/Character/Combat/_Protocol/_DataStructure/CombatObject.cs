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
        public Dictionary<CombatModuleType, OldCombatModule> ModuleTable { get; } = new();
        
        public OldDamageModule DamageModule => GetModule<OldDamageModule>(CombatModuleType.Damage);
        public OldCastingModule CastingModule => GetModule<OldCastingModule>(CombatModuleType.Casting);
        public OldCoolTimeModule CoolTimeModule => GetModule<OldCoolTimeModule>(CombatModuleType.CoolTime);
        public HealModule HealModule => GetModule<HealModule>(CombatModuleType.Heal);
        public ProjectileModule ProjectileModule => GetModule<ProjectileModule>(CombatModuleType.Projectile);
        public StatusEffectModule StatusEffectModule => GetModule<StatusEffectModule>(CombatModuleType.StatusEffect);
        public TargetModule TargetModule => GetModule<TargetModule>(CombatModuleType.Target);
        public ResourceModule ResourceModule => GetModule<ResourceModule>(CombatModuleType.Resource);
        public ProjectorModule ProjectorModule => GetModule<ProjectorModule>(CombatModuleType.Projector);
        
        public int InstanceID =>
            instanceID == 0
                ? instanceID = GetInstanceID()
                : instanceID;


        public virtual void Active() => OnActivated.Invoke();
        public virtual void Complete() => OnCompleted.Invoke();
        public virtual void Cancel() => OnCanceled.Invoke();
        public virtual void Hit() => OnHit.Invoke();
        
        
        private T GetModule<T>(CombatModuleType type) where T : OldCombatModule =>
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
