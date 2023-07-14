using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Common.UI.FloatingTexts
{
    public class FloatingText : MonoBehaviour
    {
        [SerializeField] private RectTransform textTransform;
        [SerializeField] private TextMeshProUGUI textMesh;

        private float impactScale = 2.0f;
        private float impactDuration = 0.45f;
        private float moveUpDistance = 0.35f;
        private float moveUpDuration = 0.35f;
        private float fadeDuration = 0.35f;
        private Ease moveUpEase = Ease.InCubic;
        private Vector3 randomPivot;

        private bool Initialized { get; set; }
        private CombatEntity CurrentEntity { get; set; }
        private Sequence EffectTween { get; set; }

        public ActionTable OnEnd { get; } = new();
        

        public void ImportProperty(FloatingTextEntity entity, string text)
        {
            if (entity == null) return;

            textMesh.color = entity.TextColor;
            impactScale    = entity.ImpactScale;
            impactDuration = entity.ImpactDuration;
            moveUpDistance = -entity.MoveUpDistance;
            moveUpDuration = entity.MoveUpDuration;
            fadeDuration   = entity.FadeDuration;
            moveUpEase     = entity.MoveUpEase;
            textMesh.text  = text;
            randomPivot    = Random.insideUnitSphere * entity.PivotSpreadRange;
        }
        
        public void ShowValue(CombatEntity combatEntity)
        {
            CurrentEntity = combatEntity;
                
            if (Initialized)
            {
                EffectTween.Restart();
            }
            else
            {
                EffectTween = DOTween.Sequence()
                                     .SetAutoKill(false)
                                     .Append(textTransform.DOScale(impactScale, impactDuration).From())
                                     .Append(textTransform.DOLocalMoveY(moveUpDistance, moveUpDuration)
                                                          .SetRelative()
                                                          .SetEase(moveUpEase))
                                     .Join(textMesh.DOFade(0.0f, fadeDuration))
                                     .OnComplete(OnEnd.Invoke);
                    
                Initialized = true;
            }

            SetTextPosition();
            textTransform.position = transform.position;
        }
        
        
        private void StopTween()
        {
            if (EffectTween == null) return;
            
            EffectTween.Kill();
            EffectTween = null;
            Initialized = false;
        }
        
        private void SetTextPosition()
        {
            if (CurrentEntity?.Taker == null) return;

            var targetPosition = CurrentEntity.Taker.Preposition(PrepositionType.Top).position + randomPivot;
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
    }
}
