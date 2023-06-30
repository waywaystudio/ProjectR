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

        public Image ProgressImage => progressImage;
        public FloatEvent Progress { get; set; }
        public FloatEvent Max { get; set; }

        public void Register(FloatEvent progress, FloatEvent max)
        {
            Unregister();
            
            Progress = progress;
            Max      = max;
            
            Progress.AddListener(fillProgressionKey, SetFill);
            Max.AddListener(fillProgressionKey, SetFill);
        }
        
        public void Register(FloatEvent progress, float constMax)
        {
            Unregister();
            
            Progress  = progress;
            Max       = new FloatEvent(0, constMax)
            {
                Value = constMax
            };

            Progress.AddListener(fillProgressionKey, SetFill);
            Max.AddListener(fillProgressionKey, SetFill);
            
            SetFill();
        }

        /// <summary>
        /// 값의 역으로 채울때 필요한 함수
        /// </summary>
        public void RegisterReverse(FloatEvent progress, float constMax)
        {
            Unregister();
            
            Progress = progress;
            Max = new FloatEvent(0, constMax)
            {
                Value = constMax
            };
            
            Progress.AddListener(fillProgressionKey, SetReverseFill);
            Max.AddListener(fillProgressionKey, SetReverseFill);
            
            SetReverseFill();
        }
        
        public void Unregister()
        {
            Progress?.RemoveListener(fillProgressionKey);
            Max?.RemoveListener(fillProgressionKey);
        }
        

        private void SetFill()
        {
            var clamp = Mathf.Clamp01(Progress.Value / Max.Value);

            progressImage.DOFillAmount(clamp, fillTick).SetEase(easeType);
        }

        private void SetReverseFill()
        {
            var reverseValue = Max.Value - Progress.Value;
            var clamp = Mathf.Clamp01(reverseValue / Max.Value);
            
            progressImage.DOFillAmount(clamp, fillTick).SetEase(easeType);
        }

        private void Awake()
        {
            progressImage ??= GetComponent<Image>();
        }

        private void OnDisable()
        {
            Unregister();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            TryGetComponent(out progressImage);
        }
#endif
    }
}
