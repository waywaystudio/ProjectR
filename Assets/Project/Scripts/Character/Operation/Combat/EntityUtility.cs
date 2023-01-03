using Common.Character.Operation.Combat.Entity;
using MainGame.Data.ContentData;
using UnityEngine;

namespace Common.Character.Operation.Combat
{
    public static class EntityUtility
    {
        public static void SetSkillEntity<T>(SkillData.Skill baseSkill, T entity) where T : BaseEntity
        {
            switch (entity)
            {
                case DamageEntity damageEntity:
                {
                    damageEntity.DamageValue = baseSkill.BaseValue;
                    damageEntity.Flag = EntityType.Damage;
                    break;
                }
                case CastingEntity castingEntity:
                {
                    castingEntity.OriginalCastingTime = baseSkill.CastingTime;
                    castingEntity.Flag = EntityType.Casting;
                    break;
                }
                case CoolTimeEntity coolTimeEntity:
                {
                    coolTimeEntity.CoolTime = baseSkill.BaseCoolTime;
                    coolTimeEntity.Flag = EntityType.CoolTime;
                    break;
                }
                case HealEntity healEntity:
                {
                    healEntity.HealValue = baseSkill.BaseValue;
                    healEntity.Flag = EntityType.Heal;
                    break;
                }
                case ProjectileEntity projectileEntity:
                {
                    projectileEntity.ProjectileID = (IDCode)baseSkill.ProjectileId;
                    projectileEntity.Flag = EntityType.Projectile;
                    break;
                }
                case StatusEffectEntity statusEffectEntity:
                {
                    statusEffectEntity.ActionCode = (IDCode)baseSkill.StatusEffectId;
                    statusEffectEntity.Flag = EntityType.StatusEffect;
                    break;
                }
                case TargetEntity targetEntity:
                {
                    targetEntity.TargetLayerType = baseSkill.TargetLayer;
                    targetEntity.TargetCount = baseSkill.TargetCount;
                    targetEntity.Range = baseSkill.Range;
                    targetEntity.Flag = EntityType.Target;
                    break;
                }
                case ResourceEntity resourceEntity:
                {
                    resourceEntity.Obtain = baseSkill.ResourceObtain;
                    resourceEntity.Flag = EntityType.Resource;
                    break;
                }
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
                case DamageEntity damageEntity:
                {
                    damageEntity.DamageValue = baseProjectile.BaseValue;
                    damageEntity.Flag = EntityType.Damage;
                    break;
                }
                // case CastingEntity castingEntity:
                // {
                //     castingEntity.OriginalCastingTime = baseProjectile.CastingTime;
                //     castingEntity.Flag = EntityType.Casting;
                //     break;
                // }
                // case CoolTimeEntity coolTimeEntity:
                // {
                //     coolTimeEntity.CoolTime = baseProjectile.BaseCoolTime;
                //     coolTimeEntity.Flag = EntityType.CoolTime;
                //     break;
                // }
                case HealEntity healEntity:
                {
                    healEntity.HealValue = baseProjectile.BaseValue;
                    healEntity.Flag = EntityType.Heal;
                    break;
                }
                // case ProjectileEntity projectileEntity:
                // {
                //     projectileEntity.ProjectileID = (IDCode)baseProjectile.ProjectileId;
                //     projectileEntity.Flag = EntityType.Projectile;
                //     break;
                // }
                case StatusEffectEntity statusEffectEntity:
                {
                    statusEffectEntity.ActionCode = (IDCode)baseProjectile.StatusEffectId;
                    statusEffectEntity.Flag = EntityType.StatusEffect;
                    break;
                }
                // case TargetEntity targetEntity:
                // {
                //     targetEntity.TargetLayerType = baseProjectile.TargetLayer;
                //     targetEntity.TargetCount = baseProjectile.TargetCount;
                //     targetEntity.Range = baseProjectile.Range;
                //     targetEntity.Flag = EntityType.Target;
                //     break;
                // }
                // case ResourceEntity resourceEntity:
                // {
                //     resourceEntity.Obtain = baseProjectile.ResourceObtain;
                //     resourceEntity.Flag = EntityType.Resource;
                //     break;
                // }
                default:
                {
                    Debug.LogError($"Unknown Entity. input : {entity.GetType()}");
                    break;
                }
            }
        }
    }
}
