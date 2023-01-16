using System.Collections.Generic;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Character.Combat
{
    public abstract class CombatObject : MonoBehaviour, ICombatObject, IEditorSetUp
    {
        [SerializeField] protected DataIndex actionCode;
        
        private int instanceID;

        public DataIndex ActionCode => actionCode;
        public virtual ICombatProvider Provider { get; protected set; }
        
        [ShowInInspector]
        public ActionTable OnActivated { get; } = new();
        public ActionTable OnCompleted { get; } = new();
        public ActionTable OnCanceled { get; } = new();
        public ActionTable OnHit { get; } = new();
        public List<IReady> ReadyCheckList { get; set; } = new();
        public Dictionary<ModuleType, CombatModule> ModuleTable { get; } = new();
        
        protected int InstanceID =>
            instanceID == 0
                ? instanceID = GetInstanceID()
                : instanceID;

        protected T GetModule<T>(ModuleType type) where T : CombatModule =>
            ModuleTable.ContainsKey(type)
                ? ModuleTable[type] as T
                : null;
        
        public DamageModule DamageModule => GetModule<DamageModule>(ModuleType.Damage);
        public CastingModule CastingModule => GetModule<CastingModule>(ModuleType.Casting);
        public CoolTimeModule CoolTimeModule => GetModule<CoolTimeModule>(ModuleType.CoolTime);
        public HealModule HealModule => GetModule<HealModule>(ModuleType.Heal);
        public ProjectileModule ProjectileModule => GetModule<ProjectileModule>(ModuleType.Projectile);
        public StatusEffectModule StatusEffectModule => GetModule<StatusEffectModule>(ModuleType.StatusEffect);
        public TargetModule TargetModule => GetModule<TargetModule>(ModuleType.Target);
        public ResourceModule ResourceModule => GetModule<ResourceModule>(ModuleType.Resource);
        public ProjectorModule ProjectorModule => GetModule<ProjectorModule>(ModuleType.Projector);
        
        public virtual void Active() => OnActivated.Invoke();
        public virtual void Complete() => OnCompleted.Invoke();
        public virtual void Cancel() => OnCanceled.Invoke();
        public virtual void Hit() => OnHit.Invoke();
        

#if UNITY_EDITOR
        public virtual void SetUp()
        {
            if (actionCode == DataIndex.None)
                actionCode = name.ToEnum<DataIndex>();
        }
#endif
    }
}
