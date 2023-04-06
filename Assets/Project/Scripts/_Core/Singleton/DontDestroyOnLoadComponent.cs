using UnityEngine;

namespace Singleton
{
    public class DontDestroyOnLoadComponent : MonoBehaviour
    {
        public bool ShowDebugMessage;
        public MonoBehaviour TargetBehaviour;

        private void Awake()
        {
            if (!transform.root.gameObject.Equals(gameObject))
            {
                if (ShowDebugMessage)
                {
                    Debug.LogError("Don't Destroyed Component must be in root gameObject! \n" +
                                   $"Root Object is : {transform.root.gameObject} \n" +
                                   $"Current Object is : {gameObject}");
                }

                return;
            }

            var components = FindObjectsByType(
                TargetBehaviour.GetType(), 
                FindObjectsInactive.Exclude, 
                FindObjectsSortMode.None);

            switch (components.Length)
            {
                case > 1:
                {
                    if (ShowDebugMessage)
                    {
                        Debug.Log($"{TargetBehaviour.GetType()} is came from another scene. \n" +
                                  $"In this scene {TargetBehaviour.GetType()} gameObject Destroy");
                    }

                    /* Annotation */
                    DestroyImmediate(gameObject);
                    break;
                }
                case 1:
                {
                    if (ShowDebugMessage)
                    {
                        Debug.Log($"Don't Destroyed On Load :: {TargetBehaviour.GetType()} registered");
                    }

                    DontDestroyOnLoad(gameObject);
                    break;
                }
                default:
                {
                    if (ShowDebugMessage)
                    {
                        Debug.LogError($"Can't Find {TargetBehaviour.GetType()}.");
                    }
                    
                    break;
                }
            }
        }
    }
}

 /* Annotation
  * MainGame 프리팹이 있는 씬에서 시작하여 다시 돌아올 때 새로 불려지는 씬의 MainGame 프리팹을 바로 삭제하기 위하여 
  * 해당 함수내에(38) DestroyImmediately를 사용했다. 조금 찝찝한 구석이 있다. 
  * DontDestroyOnLoadComponent 는 Script Execution Order 가 -10 으로 수동 설정되어 있다.
  */
 
 /* TargetBehaviour가 필요한 이유는, "Don't Destroy On Load Component" 가 붙어있는 게임오브젝트(보통 Main)가 
  * 여러 씬에서 개발 편의성을 위해서 활용될 경우 때문이다.
  * 새로운 씬에 이미 "Main"이 있다면 파괴해야 하는데, 중복여부를 판단하는 것이 바로 TargetBehaviour이다. 
  * 만약 TargetBehaviour를 제대로 설정하지 않는다면, 씬에 2개 오브젝트를 Don't Destroy할 경우 동일 씬 내에서 조차
  * 스타트 플레이와 함께 다른 하나를 삭제시킬 가능성이 있다.
  */