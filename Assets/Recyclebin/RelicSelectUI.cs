// using Common;
// using UnityEngine;
// using UnityEngine.UI;
//
// namespace Lobby.UI.Forge.Evolves
// {
//     public class RelicSelectUI : MonoBehaviour, IEditable
//     {
//         [SerializeField] private RelicType relicType;
//         [SerializeField] private Image iconImage;
//         [SerializeField] private Color selectedImageColor = Color.white;
//         [SerializeField] private Color unselectedImageColor = new (1, 1, 1, 0.3f);
//         
//         private bool IsFocused { get; set; }
//
//         public void SetRelic() => LobbyDirector.UI.Forge.FocusRelic = relicType;
//         public void OnSelected()
//         {
//             if (relicType == LobbyDirector.UI.Forge.FocusRelic)
//             {
//                 if (IsFocused) return;
//
//                 IsFocused = true;
//                 Select();
//             }
//             else
//             {
//                 if (!IsFocused) return;
//                 
//                 IsFocused = false;
//                 Unselect();
//             }
//         }
//         
//
//         private void Select()
//         {
//             iconImage.color      = selectedImageColor;
//         }
//
//         private void Unselect()
//         {
//             iconImage.color      = unselectedImageColor;
//         }
//
//         private void OnEnable() => Unselect();
//
//
// #if UNITY_EDITOR
//         public void EditorSetUp()
//         {
//             iconImage    = transform.Find("Icon").Find("Contents").GetComponent<Image>();
//         }
// #endif
//         
//     }
// }
