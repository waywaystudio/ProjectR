using System;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

[Serializable]
public class SinglePool<T> where T : Component
{
    [SerializeField] private GameObject prefab;

    private T singleComponent;
    private bool isInitialized;
    private IObjectPool<T> objectPool;
    
        
    public void Initialize(Action<T> createAction = null, Transform initialParent = null)
    {
        objectPool?.Clear();
        objectPool = new ObjectPool<T>(
            () => Create(createAction, initialParent), 
            component => component.gameObject.SetActive(true), 
            component => component.gameObject.SetActive(false), 
            component => Object.Destroy(component.gameObject),
            true, 
            0, 
            1);

        isInitialized = true;
    }

    public T Get()
    {
        if (!isInitialized)
        {
            Debug.LogError("PoolingSystem not Initialized. please run Initialize Function before use PoolingSystem. "
                           + $"Input:{prefab.name}");
            return null;
        }
            
        return objectPool.Get();
    }

    public void Release()
    {
        if (isInitialized)
        {
            if (singleComponent is not null)
            {
                objectPool?.Release(singleComponent);
                return;
            }

            return;
        }
            
        Debug.LogError("PoolingSystem not Initialized. please run Initialize Function before use PoolingSystem. "
                       + $"Input:{prefab.name}");
    }

    public void Clear() => objectPool.Clear();
        
        
    private T Create(Action<T> createAction, Transform parent)
    {
        if (!prefab.ValidInstantiate(out singleComponent, parent)) return null;
        
        singleComponent.gameObject.SetActive(false);
        createAction?.Invoke(singleComponent);

        return singleComponent;
    }
}