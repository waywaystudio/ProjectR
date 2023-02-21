using UnityEngine;
using UnityEngine.Pool;

namespace Core
{
    public abstract class Pool<T> : MonoBehaviour where T : class, IPoolable<T>
    {
        [SerializeField] protected GameObject prefab;
        [SerializeField] protected int maxCount;
        [SerializeField] protected Transform spawnHierarchy;

        protected IObjectPool<T> ObjectPool { get; private set; }
        protected Transform Origin => transform;

        public T Get() => ObjectPool.Get();
        public void Release(T element) => ObjectPool.Release(element);
        

        protected virtual T OnCreatePool()
        {
            if (!prefab.IsNullOrEmpty() && Instantiate(prefab, spawnHierarchy).TryGetComponent(out T component))
            {
                component.Pool = this;
                return component;
            }
            
            Debug.LogError($"Not Exist {nameof(T)} in prefab:{prefab.name}. return null");
            return null;
        }
        protected abstract void OnGetPool(T element);
        protected abstract void OnReleasePool(T element);
        protected abstract void OnDestroyPool(T element);

        private void Awake()
        {
            spawnHierarchy ??= transform;
            ObjectPool = new ObjectPool<T>(OnCreatePool,
                OnGetPool,
                OnReleasePool,
                OnDestroyPool,
                true,
                maxCount);
        }
    }
}