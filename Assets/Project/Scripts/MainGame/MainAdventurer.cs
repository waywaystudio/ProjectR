using Core;
using Core.Singleton;
using UnityEngine;

namespace MainGame
{
    public class MainAdventurer : MonoSingleton<MainAdventurer>, IEditable
    {
        [SerializeField] private GameObject knight;
        [SerializeField] private GameObject rogue;
        [SerializeField] private GameObject hunter;

        public static GameObject Knight => Instance.knight;
        public static GameObject Rogue => Instance.rogue;
        public static GameObject Hunter => Instance.hunter;

        public static bool TryGetAdventurer(DataIndex dataIndex, out GameObject adventurer)
        {
            adventurer = dataIndex switch
            {
                DataIndex.Knight => Knight,
                DataIndex.Rogue  => Rogue,
                DataIndex.Hunter => Hunter,
                _                => null,
            };
            
            if (adventurer is null) 
                Debug.LogError($"Not Combat Class Index. Input:{dataIndex}");

            return adventurer != null;
        }

        public static void Return(GameObject characterObject)
        {
            characterObject.transform.SetParent(Instance.transform);
            characterObject.SetActive(false);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            knight   = transform.Find("Knight").gameObject;
            rogue = transform.Find("Rogue").gameObject;
            hunter  = transform.Find("Hunter").gameObject;
        }
#endif
    }
}
