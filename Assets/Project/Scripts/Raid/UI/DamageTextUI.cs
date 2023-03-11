using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Raid.UI
{
    public class DamageTextUI : MonoBehaviour, IPoolable<DamageTextUI>
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
        private Sequence normalDamageEffect;
        private Sequence criticalDamageEffect;

        public Pool<DamageTextUI> Pool { get; set; }
        
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
                        .OnComplete(() => Pool.Release(this));
                    
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
                        .OnComplete(() => Pool.Release(this));

                    isNormalTweenInitialized = true;
                }
            }

            SetTextPosition();
            textTransform.position = transform.position;
        }

        
        private void SetTextPosition()
        {
            if (currentEntity == null) return;

            var targetPosition = currentEntity.Taker.DamageSpawn.position + randomPivot;
            var screenPosition = mainCamera.WorldToScreenPoint(targetPosition);

            transform.position = screenPosition;
        }

        private void Update()
        {
            SetTextPosition();
        }

        private void Awake()
        {
            mainCamera    =   Camera.main;
            damageText    ??= GetComponent<TextMeshProUGUI>();
            textTransform ??= GetComponent<RectTransform>();
        }
    }
}
