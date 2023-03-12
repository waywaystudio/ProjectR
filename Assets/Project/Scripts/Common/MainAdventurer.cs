using System.Collections.Generic;
using Singleton;
using UnityEngine;

namespace Common
{
    public class MainAdventurer : MonoSingleton<MainAdventurer>, IEditable
    {
        [SerializeField] private GameObject knight;
        [SerializeField] private GameObject rogue;
        [SerializeField] private GameObject hunter;
        
        private readonly List<GameObject> adventurerList = new();

        public static GameObject Knight => Instance.knight;
        public static GameObject Rogue => Instance.rogue;
        public static GameObject Hunter => Instance.hunter;

        public static List<GameObject> AdventurerList
        {
            get
            {
                if (Instance.adventurerList.HasElement()) 
                    return Instance.adventurerList;
                
                Instance.adventurerList.Add(Knight);
                Instance.adventurerList.Add(Rogue);
                Instance.adventurerList.Add(Hunter);

                return Instance.adventurerList;
            }
        }

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

        public static GameObject GetNext(GameObject adventurer)
        {
            if (Instance.adventurerList.IsNullOrEmpty()) return null;
            
            var index = Instance.adventurerList.FindIndex(x => x == adventurer);
            var count = Instance.adventurerList.Count;

            return Instance.adventurerList[(index + 1) % count];
        }

        public static GameObject GetPrev(GameObject adventurer)
        {
            if (Instance.adventurerList.IsNullOrEmpty()) return null;
            
            var index = Instance.adventurerList.FindIndex(x => x == adventurer);
            var count = Instance.adventurerList.Count;

            return Instance.adventurerList[(index - 1 + Instance.adventurerList.Count) % count];
        }

        public static void Return()
        {
            AdventurerList.ForEach(adventurer =>
            {
                adventurer.transform.SetParent(Instance.transform);
                adventurer.SetActive(false);
            });
        }


        protected override void Awake()
        {
            base.Awake();
            
            adventurerList.Add(Knight);
            adventurerList.Add(Rogue);
            adventurerList.Add(Hunter);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            knight = transform.Find("Knight").gameObject;
            rogue  = transform.Find("Rogue").gameObject;
            hunter = transform.Find("Hunter").gameObject;
        }
#endif
    }
}