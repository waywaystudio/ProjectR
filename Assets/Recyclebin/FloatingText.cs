// using Common;
// using DG.Tweening;
// using Sirenix.OdinInspector;
// using TMPro;
// using UnityEngine;
//
// namespace Raid.UI.FloatingTexts
// {
//     public class FloatingText : MonoBehaviour, IPoolable<FloatingText>
//     {
//         [ColorPalette("Fall"), SerializeField] private Color normalColor;
//         [ColorPalette("Fall"), SerializeField] private Color criticalColor;
//         [SerializeField] private RectTransform rectTransform;
//         [SerializeField] private TextMeshPro damageText;
//
//         private Camera mainCamera;
//         private RectTransform parentRectTransform;
//         private CombatEntity currentEntity;
//         private Sequence textEffect;
//         
//         public Pool<FloatingText> Pool { get; set; }
//
//         public void ShowValue(CombatEntity entity)
//         {
//             currentEntity = entity;
//
//             SetTextProperty(entity.Value, entity.IsCritical);
//         }
//         
//
//         private void SetTextProperty(float value, bool isEmphasis)
//         {
//             damageText.text = value.ToString("0");
//             damageText.color = isEmphasis
//                 ? criticalColor
//                 : normalColor;
//         }
//
//         private void Awake()
//         {
//             damageText          ??= GetComponent<TextMeshPro>();
//             rectTransform       ??= GetComponent<RectTransform>();
//             parentRectTransform =   GetComponentInParent<RectTransform>();
//             mainCamera          =   Camera.main;
//         }
//     }
// }