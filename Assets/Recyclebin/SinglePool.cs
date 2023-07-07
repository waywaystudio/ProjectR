// using System;
// using UnityEngine;
// using UnityEngine.Pool;
// using Object = UnityEngine.Object;
//
// [Serializable]
// public class SinglePool<T> where T : Component
// {
//     [SerializeField] protected GameObject prefab;
//     
//     /// <summary>
//     /// 게임오브젝트의 Active 상태를 풀링할 때 true(= OnGet), 릴리스 할 때 false(= OnRelease)한다.
//     /// </summary>
//     [SerializeField] protected bool syncObjectActivity = true;
//
//     protected T SingleComponent;
//     protected bool IsInitialized;
//     protected IObjectPool<T> ObjectPool;
//     
//     /// <summary>
//     /// Constructor. SinglePool<T>을 사용하기 위해서는 반드시 한 번 선행되어야 한다.
//     /// 이미 생성된 후에 다시 실행하면, 하위 Action을 갱신할 수 있다.
//     /// </summary>
//     public virtual void Initialize(Action<T> onInitialize = null, Transform parent = null, Action<T> onGet = null, Action<T> onRelease = null, Action<T> onPoolCleared = null)
//     {
//         if (syncObjectActivity)
//         {
//             onGet         += component => component.gameObject.SetActive(true);
//             onRelease     += component => component.gameObject.SetActive(false);
//             onPoolCleared += component => Object.Destroy(component.gameObject);
//         }
//             
//         ObjectPool?.Clear();
//         ObjectPool = new ObjectPool<T>(() => Create(parent, onInitialize), onGet, onRelease, onPoolCleared, 
//                                        true, 0, 1);
//
//         IsInitialized = true;
//     }
//
//     public T Get()
//     {
//         if (IsInitialized) return ObjectPool.Get();
//             
//         Debug.LogError("PoolingSystem not Initialized. please run Initialize Function before use PoolingSystem. "
//                        + $"Input:{prefab.name}");
//         return null;
//     }
//
//     public void Release()
//     {
//         if (IsInitialized)
//         {
//             ObjectPool.Release(SingleComponent);
//             return;
//         }
//             
//         Debug.LogError("PoolingSystem not Initialized. please run Initialize Function before use PoolingSystem. "
//                        + $"Input:{prefab.name}");
//     }
//
//     public void Clear() => ObjectPool.Clear();
//
//         
//         
//     protected virtual T Create(Transform parent, Action<T> onActivated)
//     {
//         if (!prefab.ValidInstantiate(out SingleComponent, parent)) return null;
//
//         onActivated?.Invoke(SingleComponent);
//         return SingleComponent;
//     }
// }
