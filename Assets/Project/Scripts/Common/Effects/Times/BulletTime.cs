using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Effects.Times
{
    public class BulletTime : MonoBehaviour
    {
        [SerializeField] private float magnification = 1f;
        [SerializeField] private float duration;
        [SerializeField] private float returnDuration = 0.1f;
        [SerializeField] private string actionKey = "PlayBulletTime";
        [SerializeField] private Section playSection;
        [SerializeField] private Section stopSection;

        private Tween bulletTimeTween;
        private bool activity = true;
        
        public bool Activity
        {
            get => activity;
            set
            {
                if (value == false)
                {
                    ReturnTime();
                }

                activity = value;
            }
        }
        
        
        public void Initialize(CombatSequence sequence)
        {
            var builder = new CombatSequenceBuilder(sequence);

            if (playSection != Section.None)
            {
                switch (playSection)
                {
                    case Section.Hit:
                    {
                        builder.AddHit(actionKey, _ => PlayBulletTime());
                        break;
                    }
                    case Section.SubHit:
                    {
                        builder.AddSubHit(actionKey, _ => PlayBulletTime());
                        break;
                    }
                    case Section.Fire:
                    {
                        builder.AddFire(actionKey, _ => PlayBulletTime());
                        break;
                    }
                    case Section.SubFire:
                    {
                        builder.AddSubFire(actionKey, _ => PlayBulletTime());
                        break;
                    }
                    default:
                    {
                        builder.Add(playSection, actionKey, PlayBulletTime);
                        break;
                    }
                }
            }
            
            if (stopSection != Section.None)
            {
                builder.Add(stopSection, actionKey, ReturnTime);
            }

            builder.Add(Section.End, actionKey, ReturnTime);
        }

        public void PlayBulletTime()
        {
            if (!activity) return;
            
            bulletTimeTween = DOTween.To(() => Time.timeScale, 
                                         x => Time.timeScale = x, 
                                         magnification, 
                                         duration)
                                     .SetEase(Ease.InOutQuad) // Change this to any easing function you prefer
                                     .SetUpdate(true);
        }

        public void ReturnTime()
        {
            if (bulletTimeTween == null) return;
            
            bulletTimeTween.Kill();
            bulletTimeTween = null;
            
            DOTween.To(() => Time.timeScale, 
                       x => Time.timeScale = x, 
                       1f, 
                       returnDuration)
                   .SetEase(Ease.InOutQuad) // Change this to any easing function you prefer
                   .SetUpdate(true);
        }


        private void OnDestroy()
        {
            if (bulletTimeTween == null) return;
            
            bulletTimeTween?.Kill();
            bulletTimeTween = null;
        }
    }
}
