using Common;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Raid.UI.FloatingTexts
{
    public class FloatingTextUI : MonoBehaviour, IEditable
    {
        [ColorPalette("Fall"), SerializeField] private Color normalColor;
        [ColorPalette("Fall"), SerializeField] private Color criticalColor;
        [SerializeField] private RectTransform textTransform;
        [SerializeField] private TextMeshProUGUI damageText;

        private bool isCriticalTweenInitialized;
        private bool isNormalTweenInitialized;
        private float normalDamageScale = 1.6f;
        private float criticalDamageScale = 2.0f;
        
        private CombatEntity currentEntity;
        private DG.Tweening.Sequence normalDamageEffect;
        private DG.Tweening.Sequence criticalDamageEffect;

        public Sequencer<CombatEntity> Sequence { get; } = new();
        public SequenceBuilder<CombatEntity> Builder { get; set; }
        public SequenceInvoker<CombatEntity> Invoker { get; set; }
        
        private static Vector3 RandomPivot => Random.insideUnitSphere * 0.5f;

        public void Initialize()
        {
            Builder = new SequenceBuilder<CombatEntity>(Sequence);
            Invoker = new SequenceInvoker<CombatEntity>(Sequence);

            Builder
                .AddActiveParam("ShowValue", ShowValue)
                ;
        }
        
        
        public void ShowValue(CombatEntity entity)
        {
            currentEntity   = entity;
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
                        .OnComplete(Invoker.End);
                    
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
                        .OnComplete(Invoker.End);

                    isNormalTweenInitialized = true;
                }
            }
            
            SetTextPosition();
            textTransform.position = transform.position;
        }

        private void StopTween()
        {
            if (criticalDamageEffect != null)
            {
                criticalDamageEffect.Kill();
                criticalDamageEffect = null;
            }

            if (normalDamageEffect != null)
            {
                normalDamageEffect.Kill();
                normalDamageEffect = null;
            }
        }

        
        private void SetTextPosition()
        {
            if (currentEntity == null) return;

            var targetPosition = currentEntity.Taker.Preposition(PrepositionType.Top).position + RandomPivot;
            var screenPosition = CameraManager.MainCamera.WorldToScreenPoint(targetPosition);

            transform.position = screenPosition;
        }

        
        private void Update()
        {
            SetTextPosition();
        }

        private void OnDestroy()
        {
            StopTween();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            damageText    = GetComponent<TextMeshProUGUI>();
            textTransform = GetComponent<RectTransform>();
        }
#endif
    }
}
