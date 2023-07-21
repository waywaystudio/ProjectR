using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Common.Projectors.Decals
{
    public class ColorChangeDecal : Decal
    {
        [SerializeField] private Color chargingColor;
        [SerializeField] private float onChangePoint;
        
        private Tween ColorTween { get; set; }
        
        
        protected override async UniTaskVoid PlayProjection()
        {
            gameObject.SetActive(true);
            DecalProjector.material.SetFloat(FillProgressShaderID, 0f);
            DecalProjector.material.SetColor(FillColorShaderID, fillColor);
            
            Cts = new CancellationTokenSource();

            var colorTrigger = true;
            var timer = 0f;
            var inverseDuration = 1.0f / Provider.CastingTime;

            while (timer < Provider.CastingTime)
            {
                timer += Time.deltaTime * inverseDuration;
                timer =  Mathf.Clamp01(timer);
                
                DecalProjector.material.SetFloat(FillProgressShaderID, timer);
                
                if (timer > Provider.CastingTime * onChangePoint && colorTrigger)
                {
                    ColorTween   = DecalProjector.material.DOColor(chargingColor, FillColorShaderID, 0.25f);
                    colorTrigger = false;
                }

                await UniTask.Yield(Cts.Token);
            }
        }
        
        protected override void StopProjection()
        {
            StopColorTween();
            
            base.StopProjection();
        }
        
        
        private void StopColorTween()
        {
            if (ColorTween == null) return;
            
            ColorTween.Kill();
            ColorTween = null;
        }
    }
}
