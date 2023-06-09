// using UnityEngine;
// using UnityEngine.Events;
// using UnityEngine.EventSystems;
// using UnityEngine.UI;
//
// namespace Lobby.UI.Forge.Upgrades
// {
//     public class ViceBubble : MonoBehaviour, IEditable, IPointerDownHandler
//     {
//         [SerializeField] private int index;
//         [SerializeField] private Image background;
//         [SerializeField] private Image boarder;
//         [SerializeField] private Image bubble;
//         [SerializeField] private Color turnOnColor = Color.red;
//         [SerializeField] private Color turnOffColor = Color.white;
//         [SerializeField] private Color disableColor = new(1, 1, 1, 0.2f);
//         [SerializeField] private UnityEvent OnClickEvent;
//
//         private bool IsOn { get; set; }
//
//         public int Index => index;
//         public bool IsActive { get; set; }
//
//         public void OnPointerDown(PointerEventData eventData)
//         {
//             OnClickEvent?.Invoke();
//         }
//
//         public void TurnOn()
//         {
//             if (IsOn) return;
//             
//             bubble.color = turnOnColor;
//             IsOn         = true;
//         }
//
//         public void TurnOff()
//         {
//             if (!IsOn) return;
//             
//             bubble.color = turnOffColor;
//             IsOn         = false;
//         }
//
//         public void Disable()
//         {
//             bubble.color = disableColor;
//         }
//
//         public void Active()
//         {
//             background.enabled = true;
//             boarder.enabled    = true;
//             bubble.enabled     = true;
//             IsActive           = true;
//         }
//
//         public void DeActive()
//         {
//             background.enabled = false;
//             boarder.enabled    = false;
//             bubble.enabled     = false;
//             IsActive           = false;
//         }
//
//
// #if UNITY_EDITOR
//         public void EditorSetUp()
//         {
//             background = GetComponent<Image>();
//             boarder    = transform.Find("Boarder").GetComponent<Image>();
//             bubble     = transform.Find("Fill").GetComponent<Image>();
//         }
// #endif
//         
//     }
// }
