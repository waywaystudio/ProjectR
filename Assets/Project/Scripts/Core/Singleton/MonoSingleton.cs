using UnityEngine;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable MemberCanBeProtected.Global

namespace Core.Singleton
{
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
                            Debug.LogError($"【{typeof(T).Name} Singleton】 Duplication. Count : {instances.Length}");
                            return instances[0] as T;
                    }
                }
            }
        }

        protected virtual void Awake()
        {
            if (isFirst)
            {
                instance = this as T;
                isFirst = false;
            }
        }

        protected virtual void OnDestroy()
        {
            if (instance == null || instance.gameObject != gameObject) 
                return;

            instance = null;
        }
    }
}