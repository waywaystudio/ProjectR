using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ImageUtility
{
    public class ImageFiller : MonoBehaviour
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
            
            Progress.Register(fillProgressionKey, SetFill);
            Max.Register(fillProgressionKey, SetFill);
        }
        
        public void Register(FloatEvent progress, float constMax)
        {
            Unregister();
            
            Progress  = progress;
            Max       = new FloatEvent(0, constMax)
            {
                Value = constMax
            };

            Progress.Register(fillProgressionKey, SetFill);
            Max.Register(fillProgressionKey, SetFill);
            
            SetFill();
        }
        
        public void Unregister()
        {
            Progress?.Unregister(fillProgressionKey);
            Max?.Unregister(fillProgressionKey);
        }
        

        private void SetFill()
        {
            var clamp = Mathf.Clamp01(Progress.Value / Max.Value);

            progressImage.DOFillAmount(clamp, fillTick).SetEase(easeType);
        }

        private void Awake()
        {
            progressImage            ??= GetComponent<Image>();
            progressImage.fillAmount =   0f;
        }

        private void OnDisable()
        {
            Unregister();
        }
        

        public void SetUp()
        {
            TryGetComponent(out progressImage);
        }
    }
}
