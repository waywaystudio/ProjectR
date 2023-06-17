using Common.Projectiles;
using UnityEngine;

namespace Common.Execution
{
    public class ProjectileExecutor : ExecuteComponent
    {
        [SerializeField] protected DataIndex actionCode;
        [SerializeField] private Transform muzzle;
        [SerializeField] private Pool<ProjectileComponent> pool;
        

        private Vector3 MuzzlePosition => muzzle ? muzzle.position : transform.position + new Vector3(0, 3, 0);
        
        public override void Execution(ICombatTaker taker)
        {
            var projectile          = pool.Get();
            var projectileTransform = projectile.transform;
            var direction           = Origin.Provider.gameObject.transform.forward;
            
            projectileTransform.SetParent(null, true);
            projectileTransform.LookAt(projectileTransform.position + direction);
            
            projectile.Activate();
        }
        

        private void CreateProjectile(ProjectileComponent projectile)
        {
            projectile.transform.position = MuzzlePosition;
            projectile.Initialize(Origin.Provider);
            projectile.Sequencer.EndAction.Add("ReturnToPool",() =>
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
