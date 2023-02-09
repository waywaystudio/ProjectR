using Character.Combat;
using Core;
using MainGame.Data.ContentData;
using UnityEngine;
using SkillData = MainGame.Data.ContentData.SkillData.Skill;

namespace Character
{
    public static class ModuleUtility
    {
        public static void SetSkillModule<T>(SkillData baseSkill, T module) where T : OldCombatModule
        {
            switch (module)
            {
                case OldDamageModule damageEntity: damageEntity.SetUpValue(baseSkill.BaseValue); break;
                case OldCastingModule castingEntity: castingEntity.SetUpValue(baseSkill.CastingTime); break;
                case OldCoolTimeModule coolTimeEntity: coolTimeEntity.SetUpValue(baseSkill.BaseCoolTime); break;
                case HealModule healEntity: healEntity.SetUpValue(baseSkill.BaseValue); break;
                case ProjectileModule projectileEntity: projectileEntity.SetUpValue(baseSkill.ProjectileId); break;
                case StatusEffectModule statusEffectEntity: statusEffectEntity.SetUpValue((DataIndex)baseSkill.StatusEffectId); break;
                case TargetModule targetEntity:
                {
                    targetEntity.SetUpValue(baseSkill.TargetLayer, baseSkill.TargetCount, baseSkill.Range, 
                                            baseSkill.SortingType.ToEnum<SortingType>(), baseSkill.IsSelf);
                    break;
                }
                case ResourceModule resourceEntity: resourceEntity.SetUpValue(baseSkill.ResourceObtain); break;
                case OldProjectorModule projectorModule: projectorModule.SetUpValue(baseSkill.ProjectorId); break;
                default:
                {
                    Debug.LogError($"Unknown Entity. input : {module.GetType()}");
                    break;
                }
            }
            
            module.SetUp();
        }
        
        public static void SetProjectileModule<T>(ProjectileData.Projectile baseProjectile, T module) where T : OldCombatModule
        {
            switch (module)
            {
                case OldDamageModule damageEntity: damageEntity.SetUpValue(baseProjectile.BaseValue);
                    break;
                case HealModule healEntity: healEntity.SetUpValue(baseProjectile.BaseValue);
                    break;
                case StatusEffectModule statusEffectEntity: statusEffectEntity.SetUpValue((DataIndex)baseProjectile.StatusEffectId); break;
                default:
                {
                    Debug.LogError($"Unknown Entity. input : {module.GetType()}");
                    break;
                }
            }
            
            module.SetUp();
        }
        
        public static void SetStatusEffectModule<T>(StatusEffectData.StatusEffect baseStatusEffect, T module) where T : OldCombatModule
        {
            switch (module)
            {
                case OldDamageModule damageEntity: damageEntity.SetUpValue(baseStatusEffect.CombatValue);
                    break;
                case HealModule healEntity: healEntity.SetUpValue(baseStatusEffect.CombatValue);
                    break;
                // case StatusEffectModule statusEffectEntity: statusEffectEntity.SetUpValue((DataIndex)baseStatusEffect.StatusEffectId); break;
                default:
                {
                    Debug.LogError($"Unknown Entity. input : {module.GetType()}");
                    break;
                }
            }
            
            module.SetUp();
        }
        
        public static void SetProjectorModule<T>(ProjectorData.Projector baseProjector, T module) where T : OldCombatModule
        {
            switch (module)
            {
                case OldDamageModule damageEntity: damageEntity.SetUpValue(baseProjector.CombatValue); break;
                case OldCastingModule damageEntity: damageEntity.SetUpValue(baseProjector.CastingTime); break;
                // case StatusEffectModule statusEffectEntity: statusEffectEntity.SetUpValue((DataIndex)baseStatusEffect.StatusEffectId); break;
                default:
                {
                    Debug.LogError($"Unknown Entity. input : {module.GetType()}");
                    break;
                }
            }
            
            module.SetUp();
        }
    }
}
