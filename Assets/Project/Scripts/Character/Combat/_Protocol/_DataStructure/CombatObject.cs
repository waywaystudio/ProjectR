using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Character.Combat
{
    public abstract class CombatObject : MonoBehaviour, IActionSender, IEditorSetUp
    {
        [SerializeField] protected DataIndex actionCode;
        
        private int instanceID;
        protected int InstanceID =>
            instanceID == 0
                ? instanceID = GetInstanceID()
                : instanceID;

        public DataIndex ActionCode => actionCode;
        public virtual ICombatProvider Provider { get; set; }
        
        protected Dictionary<ModuleType, Module> ModuleTable { get; } = new();

        protected T GetModule<T>(ModuleType type) where T : Module =>
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


        protected virtual void Awake()
        {
            GetComponents<Module>().ForEach(x => ModuleTable.Add(x.Flag, x));
        }

#if UNITY_EDITOR
        public virtual void SetUp() { }
#endif
    }
}
