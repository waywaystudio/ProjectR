using Common.Traps;
using UnityEngine;
using UnityEngine.Pool;

namespace Common.Execution
{
    public class TrapExecutor : ExecuteComponent
    {
        [SerializeField] protected DataIndex actionCode;
        [SerializeField] private GameObject prefabReference;
        [SerializeField] private int poolCapacity;
        
        private IObjectPool<TrapComponent> pool;
        
        public override void Execution(ICombatTaker taker, float instantMultiplier = 1)
        {
            Get().Activate();
        }

        private TrapComponent OnCreatePool()
        {
            if (!prefabReference.IsNullOrEmpty() && Instantiate(prefabReference).TryGetComponent(out TrapComponent component))
            {
                component.Initialize(Executor.Provider, Executor.Provider.Position);
                component.OnEnded.Register("ReturnToPool",() =>
                {
                    component.transform.position = Vector3.zero;
                    component.transform.SetParent(transform, false);
                });
                
                return component;
            }
            
            Debug.LogError($"Not Exist {nameof(TrapComponent)} in prefab:{prefabReference.name}. return null");
            return null;
        }

        private TrapComponent Get() => pool.Get();

        private void OnEnable()
        {
            pool = new ObjectPool<TrapComponent>(OnCreatePool,
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
