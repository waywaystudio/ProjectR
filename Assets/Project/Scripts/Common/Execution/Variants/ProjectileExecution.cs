using Common.Projectiles;
using UnityEngine;

namespace Common.Execution.Variants
{
    public class ProjectileExecution : FireExecution
    {
        [SerializeField] private Transform muzzle;
        [SerializeField] private Pool<ProjectileComponent> pool;

        private Vector3 MuzzlePosition => muzzle ? muzzle.position : transform.position + new Vector3(0, 3, 0);

        
        public override void Execution(Vector3 targetPosition)
        {
            var projectile = pool.Get();
            var projectileTransform = projectile.transform;

            projectileTransform.SetParent(null, true);
            projectileTransform.LookAt(new Vector3(targetPosition.x, MuzzlePosition.y, targetPosition.z));
            
            projectile.Activate();
        }
        

        private void CreateProjectile(ProjectileComponent projectile)
        {
            projectile.transform.position = MuzzlePosition;
            projectile.Initialize(Origin.Provider);
            projectile.Builder.Add(Section.End,"ReturnToPool",() =>
            {
                projectile.transform.position = MuzzlePosition;
                projectile.transform.SetParent(transform, true);

                pool.Release(projectile);
            });
        }

        private void OnEnable()
        {
            pool.Initialize(CreateProjectile, transform);
        }

        private void OnDisable()
        {
            pool.Clear();
        }
    }
}
