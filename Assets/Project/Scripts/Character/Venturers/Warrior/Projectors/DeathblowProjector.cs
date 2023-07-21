using System.Threading;
using Common.Projectors;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Projector = Common.Projectors.Projector;

namespace Character.Venturers.Warrior.Projectors
{
    public class DeathblowProjector : Projector
    {
        [SerializeField] private Color chargingColor;

        private Tween ColorTween { get; set; }
        private CancellationTokenSource cts;
        

        protected override void PlayProjection()
        {
            PlayBlowProjection().Forget();
        }
        
        protected override void StopProjection()
        {
            base.StopProjection();
            
            StopColorTween();
            StopProjectTask();
        }
        
        protected override void Dispose()
        {
            base.Dispose();

            StopProjection();
        }

        protected async UniTaskVoid PlayBlowProjection()
        {
            DecalObject.SetActive(true);
            projector.material.SetFloat(FillProgressShaderID, 0f);
            projector.material.SetColor(FillColorShaderID, fillColor);
            
            cts = new CancellationTokenSource();
            
            var timer = 0f;
            var duration = Provider.CastingTime;
            var inverseTick = 1.0f / duration;
            var colorTrigger = true;

            while (timer < duration)
            {
                timer += Time.deltaTime * inverseTick;
                
                projector.material.SetFloat(FillProgressShaderID, timer);

                if (timer > duration * 0.25f && colorTrigger)
                {
                    ColorTween   = projector.material.DOColor(chargingColor, FillColorShaderID, 0.25f);
                    colorTrigger = false;
                }
                
                await UniTask.Yield(cts.Token);
            }
        }
        
        private void StopColorTween()
        {
            if (ColorTween == null) return;
            
            ColorTween.Kill();
            ColorTween = null;
        }

        private void StopProjectTask()
        {
            if (cts == null) return;
            
            cts.Cancel();
            cts = null;
        }
    }
}
