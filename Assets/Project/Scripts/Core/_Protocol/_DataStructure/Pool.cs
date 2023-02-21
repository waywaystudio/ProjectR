using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Core
{
    public abstract class Pool<T> : MonoBehaviour where T : class, IPoolable<T>
    {
        [SerializeField] protected GameObject prefab;
        [SerializeField] protected int count;
        [SerializeField] protected Transform parent;
        [SerializeField] protected UnityEvent<T> onGet;
        [SerializeField] protected UnityEvent<T> onReleased;
        [SerializeField] protected UnityEvent<T> onDestroyed;
        
        protected IObjectPool<T> ObjectPool { get; private set; }
        protected Transform Origin => transform;

        public T Get() => ObjectPool.Get();
        public void Release(T element) => ObjectPool.Release(element);

        protected virtual T CreatePool()
        {
            if (!prefab.IsNullOrEmpty() && Instantiate(prefab, parent).TryGetComponent(out T component))
            {
                component.Pool = this;
                return component;
            }
            
            Debug.LogError($"Not Exist {nameof(T)} in prefab:{prefab.name}. return null");
            return null;
        }

        private void Awake()
        {
            parent ??= transform;
            ObjectPool = new ObjectPool<T>(CreatePool,
                onGet.Invoke,
                onReleased.Invoke,
                onDestroyed.Invoke,
                true,
                count);
        }
    }
}
