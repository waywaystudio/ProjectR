using Common.Projectiles;
using UnityEngine;
using UnityEngine.Pool;

namespace Common.Execution
{
    public class ProjectileExecutor : ExecuteComponent
    {
        [SerializeField] protected DataIndex actionCode;
        [SerializeField] private Transform muzzle;
        [SerializeField] private GameObject prefabReference;
        [SerializeField] private int poolCapacity;
        
        private IObjectPool<ProjectileComponent> pool;
        private Vector3 MuzzlePosition => muzzle ? muzzle.position : transform.position + new Vector3(0, 3, 0);
        
        public override void Execution(ICombatTaker taker, float instantMultiplier = 1f)
        {
            var projectile          = Get();
            var projectileTransform = projectile.transform;
            var direction           = Executor.Provider.gameObject.transform.forward;
            
            projectileTransform.SetParent(null, true);
            projectileTransform.LookAt(projectileTransform.position + direction);
            
            projectile.Activate();
        }
        
        private ProjectileComponent OnCreatePool()
        {
            if (!prefabReference.IsNullOrEmpty() && Instantiate(prefabReference).TryGetComponent(out ProjectileComponent component))
            {
                component.transform.position = MuzzlePosition;
                component.Initialize(Executor.Provider);
                component.OnEnded.Register("ReturnToPool",() =>
                {
                    component.transform.position = MuzzlePosition;
                    component.transform.SetParent(transform, true);

                    Release(component);
                });
                
                return component;
            }
            
            Debug.LogError($"Not Exist {nameof(ProjectileComponent)} in prefab:{prefabReference.name}. return null");
            return null;
        }
        
        private ProjectileComponent Get() => pool.Get();
        private void Release(ProjectileComponent element) => pool.Release(element);

        private void OnEnable()
        {
            pool = new ObjectPool<ProjectileComponent>(OnCreatePool,
                null,
                null,
                component => component.Dispose(),
                true,
                poolCapacity);
            
            Executor?.ExecutionTable.Add(this);
        }

        private void OnDisable()
        {
            pool.Clear();
            
            Executor?.ExecutionTable.Remove(this);
        }
    }
}
