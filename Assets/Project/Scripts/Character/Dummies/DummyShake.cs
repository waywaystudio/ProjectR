using Common;
using DG.Tweening;
using UnityEngine;

namespace Character.Dummies
{
    public class DummyShake : MonoBehaviour
    {
        [SerializeField] private Transform rootTransform;
        [SerializeField] private float duration = 0.3f;
        [SerializeField] private float strength = 0.5f;
        [SerializeField] private int vibrato = 10;

        private Tween shakeTween;

        private void Shake()
        {
            // Shake the dummy's position
            // Parameters:
            // 0.3f = duration of the shake
            // 0.5f = strength of the shake (how much it moves)
            // 10 = vibrato (how many times it shakes)
            // 90 = randomness (how random the shake is)
            // false = if true, the shake will be along the specified axis only
            // true = fadeOut (if true, the shake will fade out over time)
            shakeTween = rootTransform.DOShakePosition(duration, strength, vibrato, 90, false, true);
        }

        // TODO.Sketch 단계에서는 Self Find.
        private void Awake()
        {
            var taker = GetComponentInParent<ICombatTaker>();
            
            taker.OnCombatTaken.Add("Shake", Shake);
        }

        private void OnDestroy()
        {
            if (shakeTween == null) return;
            
            shakeTween.Kill();
            shakeTween = null;
        }
    }
}
