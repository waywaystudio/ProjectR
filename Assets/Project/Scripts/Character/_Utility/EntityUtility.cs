using Character.Combat.Skill.Modules;
using Core;
using MainGame.Data.ContentData;
using UnityEngine;

namespace Character
{
    public static class EntityUtility
    {
        public static void SetSkillEntity<T>(SkillData.Skill baseSkill, T entity) where T : SkillModule
        {
            switch (entity)
            {
                case DamageSkill damageEntity: damageEntity.SetUpValue(baseSkill.BaseValue); break;
                case CastingSkill castingEntity: castingEntity.SetUpValue(baseSkill.CastingTime); break;
                case CoolTimeSkill coolTimeEntity: coolTimeEntity.SetUpValue(baseSkill.BaseCoolTime); break;
                case HealSkill healEntity: healEntity.SetUpValue(baseSkill.BaseValue); break;
                case ProjectileSkill projectileEntity: projectileEntity.SetUpValue(baseSkill.ProjectileId); break;
                case StatusEffectSkill statusEffectEntity: statusEffectEntity.SetUpValue((DataIndex)baseSkill.StatusEffectId); break;
                case TargetSkill targetEntity:
                {
                    targetEntity.SetUpValue(baseSkill.TargetLayer, baseSkill.TargetCount, baseSkill.Range, 
                                            baseSkill.SortingType.ToEnum<SortingType>(), baseSkill.IsSelf);
                    break;
                }
                case ResourceSkill resourceEntity: resourceEntity.SetUpValue(baseSkill.ResourceObtain); break;
                default:
                {
                    Debug.LogError($"Unknown Entity. input : {entity.GetType()}");
                    break;
                }
            }
        }
        public static void SetProjectileEntity<T>(ProjectileData.Projectile baseProjectile, T entity) where T : SkillModule
        {
            switch (entity)
            {
                case DamageSkill damageEntity: damageEntity.SetUpValue(baseProjectile.BaseValue);
                    break;
                case HealSkill healEntity: healEntity.SetUpValue(baseProjectile.BaseValue);
                    break;
                case StatusEffectSkill statusEffectEntity: statusEffectEntity.SetUpValue((DataIndex)baseProjectile.StatusEffectId); break;
                default:
                {
                    Debug.LogError($"Unknown Entity. input : {entity.GetType()}");
                    break;
                }
            }
        }
    }
}
