using Character.Combat;
using Core;
using MainGame.Data.ContentData;
using UnityEngine;

namespace Character
{
    public static class ModuleUtility
    {
        public static void SetSkillModule<T>(SkillData.Skill baseSkill, T entity) where T : CombatModule
        {
            switch (entity)
            {
                case DamageModule damageEntity: damageEntity.SetUpValue(baseSkill.BaseValue); break;
                case CastingModule castingEntity: castingEntity.SetUpValue(baseSkill.CastingTime); break;
                case CoolTimeModule coolTimeEntity: coolTimeEntity.SetUpValue(baseSkill.BaseCoolTime); break;
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
                case ProjectorModule projectorModule: projectorModule.SetUpValue(baseSkill.ProjectorId); break;
                default:
                {
                    Debug.LogError($"Unknown Entity. input : {entity.GetType()}");
                    break;
                }
            }
        }
        public static void SetProjectileModule<T>(ProjectileData.Projectile baseProjectile, T entity) where T : CombatModule
        {
            switch (entity)
            {
                case DamageModule damageEntity: damageEntity.SetUpValue(baseProjectile.BaseValue);
                    break;
                case HealModule healEntity: healEntity.SetUpValue(baseProjectile.BaseValue);
                    break;
                case StatusEffectModule statusEffectEntity: statusEffectEntity.SetUpValue((DataIndex)baseProjectile.StatusEffectId); break;
                default:
                {
                    Debug.LogError($"Unknown Entity. input : {entity.GetType()}");
                    break;
                }
            }
        }
        
        public static void SetStatusEffectModule<T>(StatusEffectData.StatusEffect baseStatusEffect, T entity) where T : CombatModule
        {
            switch (entity)
            {
                case DamageModule damageEntity: damageEntity.SetUpValue(baseStatusEffect.CombatValue);
                    break;
                case HealModule healEntity: healEntity.SetUpValue(baseStatusEffect.CombatValue);
                    break;
                // case StatusEffectModule statusEffectEntity: statusEffectEntity.SetUpValue((DataIndex)baseStatusEffect.StatusEffectId); break;
                default:
                {
                    Debug.LogError($"Unknown Entity. input : {entity.GetType()}");
                    break;
                }
            }
        }
        
        public static void SetProjectorModule<T>(ProjectorData.Projector baseProjector, T module) where T : CombatModule
        {
            switch (module)
            {
                case DamageModule damageEntity: damageEntity.SetUpValue(baseProjector.CombatValue); break;
                case CastingModule damageEntity: damageEntity.SetUpValue(baseProjector.CastingTime); break;
                // case StatusEffectModule statusEffectEntity: statusEffectEntity.SetUpValue((DataIndex)baseStatusEffect.StatusEffectId); break;
                default:
                {
                    Debug.LogError($"Unknown Entity. input : {module.GetType()}");
                    break;
                }
            }
        }
    }
}
