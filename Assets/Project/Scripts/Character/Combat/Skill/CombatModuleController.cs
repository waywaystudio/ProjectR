using System.Collections.Generic;
using UnityEngine;

namespace Character.Combat.Skill
{
    public class CombatModuleController : MonoBehaviour
    {
        [SerializeField] private List<CombatModule> combatModuleList = new();
        
        public Dictionary<CombatModuleType, CombatModule> ModuleTable { get; } = new();
        public DamageModule DamageModule => Get<DamageModule>(CombatModuleType.Damage);
        public CastingModule CastingModule => Get<CastingModule>(CombatModuleType.Casting);
        public CoolTimeModule CoolTimeModule => Get<CoolTimeModule>(CombatModuleType.CoolTime);
        public GlobalCoolTimeModule GlobalCoolTimeModule => Get<GlobalCoolTimeModule>(CombatModuleType.GlobalCoolTime);
        public CollidingModule CollidingModule => Get<CollidingModule>(CombatModuleType.Colliding);
        // public HealModule HealModule => Get<HealModule>(ModuleType.Heal);
        // public ProjectileModule ProjectileModule => Get<ProjectileModule>(ModuleType.Projectile);
        // public StatusEffectModule StatusEffectModule => Get<StatusEffectModule>(ModuleType.StatusEffect);
        // public TargetModule TargetModule => Get<TargetModule>(ModuleType.Target);
        // public ResourceModule ResourceModule => Get<ResourceModule>(ModuleType.Resource);
        // public ProjectorModule ProjectorModule => Get<ProjectorModule>(ModuleType.Projector);

        private bool IsInitiated { get; set; }

        public void Initialize(CombatComponent combatComponent)
        {
            if (IsInitiated) return;
            
            combatModuleList.Clear();
            GetComponents(combatModuleList);
            
            combatModuleList.ForEach(x =>
            {
                ModuleTable.TryAdd(x.ModuleType, x);
                x.Initialize(combatComponent);
            });

            IsInitiated = true;
        }

        public bool IsReadyToActive()
        {
            if (CastingModule && CastingModule.OnCasting)
            {
                Debug.LogWarning("Casting is On");
                return false;
            }
            
            if (CoolTimeModule && !CoolTimeModule.IsReady)
            {
                Debug.LogWarning("CoolTime is Not Ready");
                return false;
            }
            
            if (GlobalCoolTimeModule && !GlobalCoolTimeModule.IsReady)
            {
                Debug.LogWarning("Global CoolTime is Not Ready");
                return false;
            }
            
            return true;
        }
        
        // public void AddModule(ModuleType type)
        // public void RemoveModule(ModuleType type)

        private T Get<T>(CombatModuleType type) where T : CombatModule =>
            ModuleTable.ContainsKey(type)
                ? ModuleTable[type] as T
                : null;


        public void SetUp()
        {
            combatModuleList.Clear();
            GetComponents(combatModuleList);
        }
    }
}
