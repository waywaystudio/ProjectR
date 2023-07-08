using System;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

[Serializable]
public class Pool<T> where T : Component
{
    [SerializeField] protected GameObject prefab;
    [SerializeField] private int maxCount = 8;
        
    /// <summary>
    /// 게임오브젝트의 Active 상태를 풀링할 때 true(= OnGet), 릴리스 할 때 false(= OnRelease)한다.
    /// </summary>
    [SerializeField] private bool syncObjectActivity = true;

    private bool isInitialized;
    private IObjectPool<T> objectPool;
    public GameObject Prefab => prefab;


    /// <summary>
    /// Constructor. Pool<T>을 사용하기 위해서는 반드시 한 번 선행되어야 한다.
    /// 이미 생성된 후에 다시 실행하면, 하위 Action을 갱신할 수 있다.
    /// </summary>
    public void Initialize(Action<T> onInitialize = null, Transform parent = null, Action<T> onGet = null, Action<T> onRelease = null, Action<T> onPoolCleared = null)
    {
        if (syncObjectActivity)
        {
            onGet         += component => component.gameObject.SetActive(true);
            onRelease     += component => component.gameObject.SetActive(false);
            onPoolCleared += component => Object.Destroy(component.gameObject);
        }
            
        objectPool?.Clear();
        objectPool = new ObjectPool<T>(
            () => Create(onInitialize, parent), 
            onGet, 
            onRelease, 
            onPoolCleared,
            true, 
            0, 
            maxCount);

        isInitialized = true;
    }

    public T Get()
    {
        if (isInitialized) return objectPool.Get();
            
        Debug.LogError("PoolingSystem not Initialized. please run Initialize Function before use PoolingSystem. "
                       + $"Input:{prefab.name}");
        return null;
    }

    public void Release(T element)
    {
        if (isInitialized)
        {
            objectPool.Release(element);
            return;
        }
            
        Debug.LogError("PoolingSystem not Initialized. please run Initialize Function before use PoolingSystem. "
                       + $"Input:{prefab.name}");
    }

    public void Clear() => objectPool.Clear();
        
        
    protected virtual T Create(Action<T> onActivated, Transform parent)
    {
        if (!prefab.ValidInstantiate(out T component, parent)) return null;

        onActivated?.Invoke(component);
        return component;
    }
}

/* Annotation
 * Create함수는 Hierarchy의 부모를 잡아주는 기능만 있다.
 * Instantiate의 추가적인 기능이 필요할 경우, Create를 오버로드하여 사용하도록 하자. 
 */