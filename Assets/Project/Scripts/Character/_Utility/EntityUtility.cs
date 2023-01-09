using Core;
using MainGame.Data.ContentData;
using UnityEngine;

namespace Character
{
    using Combat.Entities;
    
    public static class EntityUtility
    {
        public static void SetSkillEntity<T>(SkillData.Skill baseSkill, T entity) where T : BaseEntity
        {
            switch (entity)
            {
                case DamageEntity damageEntity: damageEntity.SetUpValue(baseSkill.BaseValue); break;
                case CastingEntity castingEntity: castingEntity.SetUpValue(baseSkill.CastingTime); break;
                case CoolTimeEntity coolTimeEntity: coolTimeEntity.SetUpValue(baseSkill.BaseCoolTime); break;
                case HealEntity healEntity: healEntity.SetUpValue(baseSkill.BaseValue); break;
                case ProjectileEntity projectileEntity: projectileEntity.SetUpValue(baseSkill.ProjectileId); break;
                case StatusEffectEntity statusEffectEntity: statusEffectEntity.SetUpValue((DataIndex)baseSkill.StatusEffectId); break;
                case TargetEntity targetEntity:
                {
                    targetEntity.SetUpValue(baseSkill.TargetLayer, baseSkill.Range, 
                                            baseSkill.SortingType.ToEnum<SortingType>(), baseSkill.IsSelf);
                    break;
                }
                case ResourceEntity resourceEntity: resourceEntity.SetUpValue(baseSkill.ResourceObtain); break;
                default:
                {
                    Debug.LogError($"Unknown Entity. input : {entity.GetType()}");
                    break;
                }
            }
        }
        public static void SetProjectileEntity<T>(ProjectileData.Projectile baseProjectile, T entity) where T : BaseEntity
        {
            switch (entity)
            {
                case DamageEntity damageEntity: damageEntity.SetUpValue(baseProjectile.BaseValue);
                    break;
                case HealEntity healEntity: healEntity.SetUpValue(baseProjectile.BaseValue);
                    break;
                case StatusEffectEntity statusEffectEntity: statusEffectEntity.SetUpValue((DataIndex)baseProjectile.StatusEffectId); break;
                default:
                {
                    Debug.LogError($"Unknown Entity. input : {entity.GetType()}");
                    break;
                }
            }
        }
    }
}
