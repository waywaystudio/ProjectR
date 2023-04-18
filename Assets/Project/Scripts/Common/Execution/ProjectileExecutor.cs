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
        
        public override void Execution(ICombatTaker taker, float instantMultiplier = 1f)
        {
            var projectile          = pool.Get();
            var projectileTransform = projectile.transform;
            var direction           = Executor.Provider.gameObject.transform.forward;
            
            projectileTransform.SetParent(null, true);
            projectileTransform.LookAt(projectileTransform.position + direction);
            
            projectile.Activate();
        }
        

        private void CreateProjectile(ProjectileComponent projectile)
        {
            projectile.transform.position = MuzzlePosition;
            projectile.Initialize(Executor.Provider);
            projectile.OnEnded.Register("ReturnToPool",() =>
            {
                projectile.transform.position = MuzzlePosition;
                projectile.transform.SetParent(transform, true);

                pool.Release(projectile);
            });
        }

        private void OnEnable()
        {
            pool.Initialize(CreateProjectile, transform);
            Executor?.ExecutionTable.Add(this);
        }

        private void OnDisable()
        {
            pool.Clear();
            Executor?.ExecutionTable.Remove(this);
        }
    }
}
