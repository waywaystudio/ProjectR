using DG.Tweening;
using SceneAdaption;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
    public class FadePanel : MonoBehaviour, ISceneChangeHandler
    {
        [SerializeField] private Image panel;

        public void OnChanging()
        {
            // FadeOut
            panel.DOFade(0.0f, 0f);
            panel.enabled = true;
            panel.DOFade(1.0f, 0.2f)
                 .OnComplete(() => panel.enabled = false);
        }

        public void OnChanged()
        {
            // FadeIn
            panel.DOFade(1.0f, 0f);
            panel.enabled = true;
            panel.DOFade(0.0f, 0.2f)
                 .OnComplete(() => panel.enabled = false);
        }
        
        
        private void Awake()
        {
            panel.enabled = false;
        }
    }
}
