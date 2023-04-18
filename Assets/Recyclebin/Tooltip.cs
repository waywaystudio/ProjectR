// using TMPro;
// using UnityEngine;
//
// namespace Common.UI.Tooltips
// {
//     public class Tooltip : MonoBehaviour, IPoolable<Tooltip>, IEditable
//     {
//         [SerializeField] private TextMeshProUGUI textMesh;
//
//         public OldPool<Tooltip> Pool { get; set; }
//
//         public void Show(string info)
//         {
//             textMesh.text = info;
//         }
//
//
// #if UNITY_EDITOR
//         public void EditorSetUp()
//         {
//             textMesh ??= GetComponentInChildren<TextMeshProUGUI>();
//         }
// #endif
//         
//     }
// }
