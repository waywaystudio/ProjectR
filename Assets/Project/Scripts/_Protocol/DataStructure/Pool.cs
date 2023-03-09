using UnityEngine;
using UnityEngine.Pool;

namespace Core
{
    public abstract class Pool<T> : MonoBehaviour where T : class, IPoolable<T>
    {
        [SerializeField] protected GameObject prefab;
        [SerializeField] protected int maxCount;

        protected IObjectPool<T> ObjectPool { get; private set; }
        protected Transform Origin => transform;

        public T Get() => ObjectPool.Get();
        public void Release(T element) => ObjectPool.Release(element);
        

        protected T OnCreatePool()
        {
            if (!prefab.IsNullOrEmpty() && Instantiate(prefab).TryGetComponent(out T component))
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

        protected virtual void Awake()
        {
            ObjectPool = new ObjectPool<T>(OnCreatePool,
                OnGetPool,
                OnReleasePool,
                OnDestroyPool,
                true,
                maxCount);
        }


        public void SetProperties(GameObject prefab, int maxCount)
        {
            this.prefab   = prefab;
            this.maxCount = maxCount;
        }
    }
}
