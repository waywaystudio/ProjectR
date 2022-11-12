using UnityEngine;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable MemberCanBeProtected.Global

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private static object @lock = new ();
    private static bool isFirst = true;

    public static T Instance
    {
        get
        {
            lock (@lock)
            {
                if (instance != null) 
                    return instance;

                var instances = FindObjectsOfType(typeof(T));

                switch (instances.Length)
                {
                    case 0: return null;
                    case 1: return instance = instances[0] as T;
                    default:
#if (DEBUG_MODE)
                            Debug.LogError($"【{type.Name} Singleton】 Duplication. Count : {instances.Length}");
#endif
                        return instances[0] as T;
                }
            }
        }
    }

    [System.Diagnostics.Conditional("DEBUG_MODE")]
    protected virtual void Awake()
    {
        if (isFirst)
        {
            instance = this as T;
            isFirst = false;
#if (DEBUG_MODE)
                if (instance != null)
                    Debug.Log($"[Singleton] Create instance at firstTime : 【{instance.GetType().Name}】");
#endif
        }
        else
        {
            if (instance != null)
            {
                Debug.Log(
                    $"【{instance.GetType().Name}】 singleton is already Exist.\n" +
                    $"Called From【{instance.gameObject.name}】 gameObject.\n");
            }
            else
                instance = this as T;
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == null || instance.gameObject != gameObject) return;
#if (DEBUG_MODE)
            Debug.Log($"[Singleton] Destroy instance : 【{instance.GetType().Name}】");
#endif
        instance = null;
    }
}