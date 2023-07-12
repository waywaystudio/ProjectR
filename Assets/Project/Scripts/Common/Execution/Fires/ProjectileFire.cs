using Common.Projectiles;
using UnityEngine;

namespace Common.Execution.Fires
{
    public class ProjectileFire : FireExecution
    {
        [SerializeField] private Transform muzzle;
        [SerializeField] private Pool<Projectile> pool;

        private Vector3 MuzzlePosition => muzzle 
            ? muzzle.position 
            : transform.position + new Vector3(0, 3, 0);

        
        public override void Fire(Vector3 targetPosition)
        {
            var projectile = pool.Get();
            var projectileTransform = projectile.transform;

            projectileTransform.SetParent(null, true);
            projectileTransform.LookAt(new Vector3(targetPosition.x, MuzzlePosition.y, targetPosition.z));
            
            projectile.Invoker.Active(targetPosition);
        }
        

        protected virtual void CreateProjectile(Projectile projectile)
        {
            projectile.transform.position = MuzzlePosition;
            projectile.Initialize(Sender.Provider);
            projectile.Builder
                      .Add(Section.End,"ReturnToPool", () => ReturnToPool(projectile));
        }

        private void ReturnToPool(Projectile projectile)
        {
            projectile.transform.position = MuzzlePosition;
            projectile.transform.SetParent(transform, true);
            
            pool.Release(projectile);
        }

        private void OnEnable()
        {
            pool.Initialize(CreateProjectile, transform);
        }
    }
}
