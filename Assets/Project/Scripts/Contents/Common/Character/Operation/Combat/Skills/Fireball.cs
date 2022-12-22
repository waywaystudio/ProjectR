
using Common.Character.Operation.Combat.Entity;
using UnityEngine;

namespace Common.Character.Operation.Combat.Skills
{
    public class Fireball : BaseSkill
    {
        [SerializeField] private float combatValue;
        
        public override CombatValueEntity CombatValue
        {
            get
            {
                var damageValue = Cb.CombatValue;
                damageValue.Power = Cb.CombatValue.Power * combatValue;

                return damageValue;
            }
        }
        
        public override void CompleteSkill()
        {
            var hasDamageProvider = TryGetComponent(out DamageEntity damageEntity);
            var hasStatusEffectProvider = TryGetComponent(out StatusEffectEntity statusEffectEntity);
            var hasTargetList = TryGetComponent(out TargetEntity targetEntity);
            var hasProjectile = TryGetComponent(out ProjectileEntity projectileEntity);

            if (hasTargetList && hasProjectile)
            {
                targetEntity.CombatTakerList.ForEach(target =>
                {
                    projectileEntity.Initialize(target);

                    if (hasDamageProvider)
                    {
                        projectileEntity.OnArrived.Register
                            (damageEntity.GetInstanceID(), () => target.TakeDamage(damageEntity));
                    }

                    if (hasStatusEffectProvider)
                    {
                        projectileEntity.OnArrived.Register
                            (statusEffectEntity.GetInstanceID(), () => target.TakeStatusEffect(statusEffectEntity.Origin));
                    }
                });
            }

            base.CompleteSkill();
        }
    }
}
