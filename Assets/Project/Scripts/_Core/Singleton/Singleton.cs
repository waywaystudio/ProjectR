using UnityEngine;

namespace Singleton
{
    public class Singleton<T> where T : class, new()
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void RuntimeInitialize() => instance = null;
        
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();

                    return instance;
                }

                return instance;
            }
        }

        protected Singleton() { }
    }
}