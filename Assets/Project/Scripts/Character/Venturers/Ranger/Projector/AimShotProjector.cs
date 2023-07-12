using System.Threading;
using Common;
using Common.Projectors;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Character.Venturers.Ranger.Projector
{
    public class AimShotProjector : LineProjector
    {
        [SerializeField] private float fullChargingScale = 1.5f;
        [SerializeField] private Color chargingColor;

        private Tween headColorTween;
        private Tween bodyColorTween;
        
        public override void Initialize(IProjectionProvider provider)
        {
            base.Initialize(provider);
            
            var builder = new CombatSequenceBuilder(provider.Sequence);

            builder
                .Add(Section.Cancel, "ShaderProgression", StopTween)
                .Add(Section.End, "ShaderProgression", StopTween);
        }
        
        protected override void Dispose()
        {
            base.Dispose();

            StopTween();
        }
        
        
        protected override async UniTaskVoid PlayProjector(float duration)
        {
            Cts = new CancellationTokenSource();
            
            var timer = 0f;
            var chargingLength = fullChargingScale * length;
            var gap = chargingLength - length;
            var colorTrigger = true;
            
            // 1 + 0.5 / 1
            // 1 + 0.5 / timer;

            while (timer < duration)
            {
                timer  += Time.deltaTime * 1.0f / duration;
                var scaler = length + gap * timer;

                SizeModifier(scaler, width);
                projector.material.SetFloat(FillProgressShaderID, Mathf.Clamp01(-1f + timer * 2));
                bodyProjector.material.SetFloat(FillProgressShaderID, Mathf.Clamp01(timer * 2));

                // Over HalfProgression
                if (timer > duration * 0.25f && colorTrigger)
                {
                    headColorTween = projector.material.DOColor(chargingColor, FillColorShaderID, 0.25f);
                    bodyColorTween = bodyProjector.material.DOColor(chargingColor, FillColorShaderID, 0.25f);

                    colorTrigger = false;
                }
                
                await UniTask.Yield(Cts.Token);
            }
        }
        
        protected override void StopProjection()
        {
            base.StopProjection();
            
            projector.material.SetColor(FillColorShaderID, fillColor);
            bodyProjector.material.SetColor(FillColorShaderID, fillColor);
        }

        private void StopTween()
        {
            if (headColorTween != null)
            {
                headColorTween.Kill();
                headColorTween = null;
            }
            
            if (bodyColorTween != null)
            {
                bodyColorTween.Kill();
                bodyColorTween = null;
            }
        }

        private void SizeModifier(float length, float width)
        {
            var headSize = projector.size;
            
            headSize.x = length + 1 + (width - 1);
            headSize.y = length + 1;
            headSize.z = ProjectorDepth;
            
            projector.pivot = new Vector3(0, 0,ProjectorDepth / 2);
            projector.size  = headSize;
            projector.transform.localPosition =
                new Vector3(0, YPosition, (length + 1) * 0.5f * ArrowZDisplacement);
            
            var bodySize = bodyProjector.size;
            
            bodySize.x = length + 1 + (width - 1);
            bodySize.y = length + 1;
            bodySize.z = ProjectorDepth;
            
            bodyProjector.pivot = new Vector3(0, 0,ProjectorDepth / 2);
            bodyProjector.size  = bodySize;
            bodyProjector.transform.localPosition = 
                new Vector3(0, YPosition, (length + 1) * 0.5f);
        }
    }
}
