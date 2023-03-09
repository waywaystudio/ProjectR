using UnityEngine;

namespace UI
{
    public class FadePanel : MonoBehaviour
    {
        [SerializeField] private Animator fadeinAnimator;

        private static readonly int FadeIn = Animator.StringToHash("fadeIn");
        private static readonly int FadeOut = Animator.StringToHash("fadeOut");

        private void Awake()
        {
            fadeinAnimator = GetComponent<Animator>();
        }

        public void PlayFadeIn()
        {
            fadeinAnimator.SetBool(FadeIn, true);
        }

        public void PlayFadeOut()
        {
            fadeinAnimator.SetBool(FadeOut, true);
        }

        // Animation::FadeInAnimation
        public void InitAnimation()
        {
            fadeinAnimator.SetBool(FadeIn, false);
            fadeinAnimator.SetBool(FadeOut, false);
        }

        // Animation::FadeInAnimation
        public void FadeInDone()
        {
            fadeinAnimator.SetBool(FadeIn, false);
        }
    
        // Animation::FadeOutAnimation
        public void FadeOutDone()
        {
            fadeinAnimator.SetBool(FadeOut, false);
        }
    }
}
