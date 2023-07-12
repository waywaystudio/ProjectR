using Character.Venturers.Ranger.Skills;
using Common;
using Common.Execution.Fires;
using Common.Execution.Hits;
using Common.Projectiles;
using UnityEngine;

namespace Character.Venturers.Ranger.Executions
{
    /*
     * Charging Progress Damage 구현을 위해 특수 제작.
     */
    public class AimShotFire : ProjectileFire
    {
        private DamageHit damageHit;
        
        public override void Fire(Vector3 targetPosition)
        {
            base.Fire(targetPosition);

            if (Sender.Provider.SkillTable[DataIndex.AimShot] is not AimShot aimShot) return;
            
            var originalDamage = damageHit.DamageSpec.Power;
            var executedValue = aimShot.ExecuteProgression;
            var modifiedPower = originalDamage * executedValue;

            damageHit.DamageSpec.Change(StatType.Power, modifiedPower);
        }
        
        protected override void CreateProjectile(Common.Projectiles.Projectile projectile)
        {
            base.CreateProjectile(projectile);

            damageHit = projectile.HitExecutor.OfType<DamageHit>();

            var originalDamage = damageHit.DamageSpec.Power;
            
            projectile.Builder
                      .Add(Section.End,"ReturnToOriginalPower", 
                           () => damageHit.DamageSpec.Change(StatType.Power, originalDamage));
        }
    }
}
