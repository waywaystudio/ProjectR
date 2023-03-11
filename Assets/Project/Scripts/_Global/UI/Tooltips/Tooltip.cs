using TMPro;
using UnityEngine;

namespace UI.Tooltips
{
    public class Tooltip : MonoBehaviour, IPoolable<Tooltip>, IEditable
    {
        [SerializeField] private TextMeshProUGUI textMesh;

        public Pool<Tooltip> Pool { get; set; }

        public void Show(string info)
        {
            textMesh.text = info;
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            textMesh ??= GetComponentInChildren<TextMeshProUGUI>();
        }
#endif
        
    }
}
