using Common;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Raid.UI.FloatingTexts
{
    public class FloatingTextUI : MonoBehaviour, IOldEndSection
    {
        [ColorPalette("Fall"), SerializeField] private Color normalColor;
        [ColorPalette("Fall"), SerializeField] private Color criticalColor;
        [SerializeField] private RectTransform textTransform;
        [SerializeField] private TextMeshProUGUI damageText;

        private bool isCriticalTweenInitialized;
        private bool isNormalTweenInitialized;
        private float normalDamageScale = 1.6f;
        private float criticalDamageScale = 2.0f;
        
        private Vector3 randomPivot;
        private Camera mainCamera;
        private CombatEntity currentEntity;
        private DG.Tweening.Sequence normalDamageEffect;
        private DG.Tweening.Sequence criticalDamageEffect;

        public ActionTable OnEnded { get; } = new();
        
        
        public void ShowValue(CombatEntity entity)
        {
            currentEntity = entity;
            randomPivot   = Random.insideUnitSphere * 0.5f;
            
            damageText.text = entity.Value.ToString("0");

            if (entity.IsCritical)
            {
                damageText.color = criticalColor;
                
                if (isCriticalTweenInitialized)
                {
                    criticalDamageEffect.Restart();
                }
                else
                {
                    criticalDamageEffect = DOTween.Sequence()
                        .SetAutoKill(false)
                        .Append(textTransform.DOScale(criticalDamageScale, 0.45f).From())
                        .Append(textTransform.DOLocalMoveY(-25.0f, 0.35f).SetRelative().SetEase(Ease.InCubic))
                        .Join(damageText.DOFade(0.0f, 0.35f))
                        .OnComplete(OnEnded.Invoke);
                    
                    isCriticalTweenInitialized = true;
                }
            }
            else
            {
                damageText.color = normalColor;

                if (isNormalTweenInitialized)
                {
                    normalDamageEffect.Restart();
                }
                else
                {
                    normalDamageEffect = DOTween.Sequence()
                        .SetAutoKill(false)
                        .Append(textTransform.DOScale(normalDamageScale, 0.35f).From())
                        .Append(textTransform.DOLocalMoveY(-15.0f, 0.35f).SetRelative().SetEase(Ease.InCubic))
                        .Join(damageText.DOFade(0.0f, 0.35f))
                        .OnComplete(OnEnded.Invoke);

                    isNormalTweenInitialized = true;
                }
            }
            
            SetTextPosition();
            textTransform.position = transform.position;
        }

        
        private void SetTextPosition()
        {
            if (currentEntity == null) return;

            var targetPosition = currentEntity.Taker.Preposition(PrepositionType.Top).position + randomPivot;
            var screenPosition = mainCamera.WorldToScreenPoint(targetPosition);

            transform.position = screenPosition;
        }

        private void Awake()
        {
            mainCamera    =   Camera.main;
            damageText    ??= GetComponent<TextMeshProUGUI>();
            textTransform ??= GetComponent<RectTransform>();
        }
        
        private void Update()
        {
            SetTextPosition();
        }
    }
}
