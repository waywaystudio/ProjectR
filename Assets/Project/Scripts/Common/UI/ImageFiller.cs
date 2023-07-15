using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
    public class ImageFiller : MonoBehaviour, IEditable
    {
        [SerializeField] private Image progressImage;
        [SerializeField] private string fillProgressionKey;
        [SerializeField] private float fillTick;
        [SerializeField] private Ease easeType;

        public FloatEvent Progress { get; set; }
        public FloatEvent Max { get; set; }

        private TimeTrigger Trigger { get; set; }
        private Tween FillTween { get; set; }


        /// <summary>
        /// Casting, CoolTime
        /// </summary>
        public void Register(TimeTrigger trigger)
        {
            Unregister();

            if (trigger.Duration == 0f)
            {
                progressImage.fillAmount = 0f;
                return;
            }

            Trigger = trigger;
            Trigger.AddListener(fillProgressionKey, Fill);

            Fill(Trigger.Timer);
        }
        
        /// <summary>
        /// Hp, Resource tracking
        /// </summary>
        public void Register(FloatEvent progress, float constMax)
        {
            UnregisterFloatEvent();
            
            Progress = progress;
            Max      = new FloatEvent(constMax);

            Progress.AddListener(fillProgressionKey, SetFill);
            Max.AddListener(fillProgressionKey, SetFill);
            SetFill();
        }

        public void Unregister()
        {
            Trigger?.RemoveListener(fillProgressionKey);
        }
        

        private void Fill(float triggerTimer)
        {
            var progress = Mathf.Clamp01(triggerTimer / Trigger.Duration);

            progressImage.fillAmount = progress;
        }

        private void OnDisable()
        {
            UnregisterFloatEvent();
        }

        private void OnDestroy()
        {
            if (FillTween == null) return;
            
            FillTween.Kill();
            FillTween = null;
        }
        

        /*----------------------------*/
        /// <summary>
        /// 값의 역으로 채울때 필요한 함수.
        /// Hp, resource Empty UI에 사용.
        /// </summary>
        public void RegisterReverse(FloatEvent progress, float constMax)
        {
            UnregisterFloatEvent();
            
            Progress = progress;
            Max      = new FloatEvent(constMax);
            
            Progress.AddListener(fillProgressionKey, SetReverseFill);
            Max.AddListener(fillProgressionKey, SetReverseFill);
            
            SetReverseFill();
        }
        
        public void UnregisterFloatEvent()
        {
            Progress?.RemoveListener(fillProgressionKey);
            Max?.RemoveListener(fillProgressionKey);
        }
        
        private void SetFill()
        {
            if (Max.Value == 0.0f)
            {
                progressImage.fillAmount = 0f;
                return;
            }
            
            var clamp = Mathf.Clamp01(Progress.Value / Max.Value);

            progressImage.DOFillAmount(clamp, fillTick).SetEase(easeType);
        }

        private void SetReverseFill()
        {
            var reverseValue = Max.Value - Progress.Value;
            var clamp = Mathf.Clamp01(reverseValue / Max.Value);
            
            progressImage.DOFillAmount(clamp, fillTick).SetEase(easeType);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            TryGetComponent(out progressImage);
        }
#endif
    }
}
