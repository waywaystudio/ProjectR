using UnityEngine;

namespace Common.Characters
{
    public class CharacterPreposition : MonoBehaviour, IEditable
    {
        [SerializeField] private Table<PrepositionType, Transform> table;
        
        public Transform Get(PrepositionType type) => table[type];


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            table.Clear();

            var topTransform = transform.Find("Top").transform;
            var headTransform = transform.Find("Head").transform;
            var bodyTransform = transform.Find("Body").transform;
            
            table.Add(PrepositionType.Top, topTransform);
            table.Add(PrepositionType.Head, headTransform);
            table.Add(PrepositionType.Body, bodyTransform);
            table.Add(PrepositionType.Origin, transform);
        }
#endif
    }
}
