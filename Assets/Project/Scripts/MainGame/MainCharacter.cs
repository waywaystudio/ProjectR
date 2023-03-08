using Core;
using Core.Singleton;
using UnityEngine;

namespace MainGame
{
    public class MainCharacter : MonoSingleton<MainCharacter>, IEditable
    {
        [SerializeField] private GameObject knight;
        [SerializeField] private GameObject assassin;
        [SerializeField] private GameObject soldier;

        public static GameObject Knight => Instance.knight;
        public static GameObject Assassin => Instance.assassin;
        public static GameObject Soldier => Instance.soldier;
        

        public static void ReturnToOriginalHierarchy(GameObject characterObject)
        {
            characterObject.transform.SetParent(Instance.transform);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            knight   = transform.Find("Knight").gameObject;
            assassin = transform.Find("Assassin").gameObject;
            soldier  = transform.Find("Soldier").gameObject;
        }
#endif
    }
}
