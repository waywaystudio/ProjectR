using Core;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Raid.UI
{
    public class DamageTextUI : MonoBehaviour, IPoolable<DamageTextUI>
    {
        [ColorPalette("Fall"), SerializeField] private Color normalColor;
        [ColorPalette("Fall"), SerializeField] private Color criticalColor;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private TextMeshProUGUI damageText;

        private CombatEntity currentEntity;
        private Sequence textEffect;
        private ICombatTaker taker;

        public Pool<DamageTextUI> Pool { get; set; }
        
        public void ShowValue(CombatEntity entity)
        {
            currentEntity = entity;
            taker         = entity.Taker;

            SetTextProperty(entity.Value, entity.IsCritical);
        }
        
        private void SetTextProperty(float value, bool isEmphasis)
        {
            damageText.text = value.ToString("0");
            damageText.color = isEmphasis
                ? criticalColor
                : normalColor;
        }

        private void Awake()
        {
            damageText    ??= GetComponent<TextMeshProUGUI>();
            rectTransform ??= GetComponent<RectTransform>();
            
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
