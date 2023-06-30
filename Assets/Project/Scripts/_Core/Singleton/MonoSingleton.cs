using UnityEngine;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable MemberCanBeProtected.Global

namespace Singleton
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance is not null) 
                    return instance;

                var instances = FindObjectsByType<T>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

                switch (instances.Length)
                {
                    case 0: return null;
                    case 1: return instance = instances[0];
                    default:
                        Debug.LogError($"【{typeof(T).Name} Singleton】 Duplication. Count : {instances.Length}");
                        return instances[0];
                }
            }
        }


        protected virtual void Awake()
        {
            instance = this as T;
        }

        protected virtual void OnDestroy()
        {
            if (instance == null || instance.gameObject != gameObject) 
                return;

            instance = null;
        }
        
        // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ResetSingleton()
        {
            instance = null;
        }
    }
}

/* Annotation I
 * MonoSingleton에 대한 자료를 찾아보면 lock을 활용하는 경우가 많다.
 * 본 스크립트는 lock을 사용하지 않으며 이유는 다음과 같다.
 *
 * Unity does support multithreading, but it is important to note that Unity's API
 * (e.g., MonoBehaviour functions and most of the UnityEngine namespace)
 * is not thread-safe and should only be accessed from the main thread.
 * This means you should be cautious when using multithreading in your Unity projects to avoid unexpected behavior or crashes.
 *
 * That being said, there are still scenarios where you might use multithreading in Unity,
 * such as loading resources, performing complex calculations, or handling network communication in the background.
 * In such cases, using synchronization mechanisms like lock can be useful to ensure thread safety when accessing shared resources.
 *
 * However, in the context of the Singleton pattern and Unity,
 * the lock statement can be considered unnecessary most of the time.
 * This is because Unity's API calls, including the MonoBehaviour methods, run on the main thread,
 * and thus there is no concurrent access to the Singleton instance.
 * If you're only accessing the Singleton instance from MonoBehaviour methods
 * and not using any custom threading in your project, you can safely remove the lock statement without any issues.
 * 
 * To summarize,
 * Unity does support multithreading, but most of the Unity API is not thread-safe and should only be accessed from the main thread.
 * Using the lock statement can be useful in some cases,
 * but it is often unnecessary for a Singleton implementation in Unity,
 * given that you're only accessing the Singleton from MonoBehaviour methods.
 *
 * Unity는 멀티스레딩을 지원하지만 많은 유니티 API함수들이 멀티 스레딩에 안전하지 못하다.
 * 싱글톤에 lock을 사용하는 기조라면, 다른 많은 API함수들도 스레드 세이프 하게 다루어야 하는데 보통 그렇지 않다.
 * 이런 상황에서 굳이 싱글톤만 lock을 사용한다는 것은 어불성설이다.
 * 따라서 lock을 사용하지 않는다.
 */
 
 /* Annotation II
  * 굳이 Awake에서 instance = this as T; 를 활용하여 유니티 이벤트를 사용하는 이유는,
  * Instance Property에 FindObjectsByType 이하 구문을 사용하지 않기 위해서다.
  * 따라서 MonoSingleton의 Awake보다 먼저 Singleton에 접근하여 Instance를 호출하는 경우에는
  * FindObjectsByType 이하 구문을 통해 instance가 할당되며 Awake에서 할당하는 의미가 약해진다.
  * 만약 이런 부분이 자주 발생하며 통제가 불가능할 경우 MonoSingleton의 스크립트 실행 순서를 -10 ~ 0 사이로 잡아주거나
  * 차라리 Awake를 날려버리자.
  */
  
  /* Annotation III
   * FindObjectsByType은 FindObjectOfType의 업그레이드 버전으로, 2021.3버전에서 추가되었다.
   * Of에 비해서 최적화 되었으며 사실상 Of는 Obsolete라고 한다.
   */