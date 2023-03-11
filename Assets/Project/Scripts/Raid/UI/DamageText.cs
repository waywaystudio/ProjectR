using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Raid.UI
{
    public class DamageText : MonoBehaviour, IPoolable<DamageText>
    {
        [ColorPalette("Fall"), SerializeField] private Color normalColor;
        [ColorPalette("Fall"), SerializeField] private Color criticalColor;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private TextMeshPro damageText;

        private Camera mainCamera;
        private RectTransform parentRectTransform;
        private CombatEntity currentEntity;
        private Sequence textEffect;
        
        public Pool<DamageText> Pool { get; set; }

        public void ShowValue(CombatEntity entity)
        {
            currentEntity = entity;

            SetTextProperty(entity.Value, entity.IsCritical);
            
            
            // SetTextLocalPosition(entity.Taker.DamageSpawn.position);
            // var offsetPosition = new Vector3(rectTransform.localPosition.x + 50, rectTransform.localPosition.y + 50,0);
            // textEffect.Restart();
        }
        

        private void SetTextProperty(float value, bool isEmphasis)
        {
            damageText.text = value.ToString("0");
            damageText.color = isEmphasis
                ? criticalColor
                : normalColor;
        }

        // private void SetTextLocalPosition(Vector3 worldPosition)
        // {
        //     var screenPoint = mainCamera.WorldToScreenPoint(worldPosition);
        //
        //     RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, null, out var canvasPos);
        //
        //     rectTransform.localPosition = canvasPos;
        // }

        private void Awake()
        {
            damageText          ??= GetComponent<TextMeshPro>();
            rectTransform       ??= GetComponent<RectTransform>();
            parentRectTransform =   GetComponentInParent<RectTransform>();
            mainCamera          =   Camera.main;
            
            // textEffect = DOTween.Sequence()
            //     .SetAutoKill(false)
            //     .Append(rectTransform.DOScale(2.0f, 0.8f).SetEase(Ease.OutCubic))
            //     .Join(rectTransform.DOLocalMoveY(50.0f, 0.8f).SetRelative().SetEase(Ease.OutCubic))
            //     .OnComplete(() =>
            //     {
            //         Pool.Release(this);
            //         textEffect.Rewind();
            //     });

            // .Join(rectTransform.DOLocalMove(offset, 0.8f).SetRelative().SetEase(Ease.InCubic))
            // .Append(rectTransform.DOLocalMove(offset, 0.8f).SetRelative().SetEase(Ease.OutCirc))
        }
    }
}
