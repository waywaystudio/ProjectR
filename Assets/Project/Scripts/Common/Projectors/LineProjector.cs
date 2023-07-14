using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Common.Projectors
{
    public class LineProjector : ProjectorComponent
    {
        [SerializeField] protected Material bodyMaterial;
        [SerializeField] protected DecalProjector bodyProjector;
        [SerializeField] protected float fadeAmount;
        [SerializeField] protected float length;
        [SerializeField] protected float width;
        
        /// <summary>
        /// The line head projector's Z offset, this lines up the head and body of the line. 
        /// </summary>
        protected const float ArrowZDisplacement = 2.9835f;

        /// <summary>
        /// The projector's 'y' position.
        /// </summary>
        protected const float YPosition = 0.15f;
        
        /// <summary>
        /// The shader ID of the <c>_FadeAmount</c> property.
        /// </summary>
        private static readonly int FadeAmountShaderID = Shader.PropertyToID("_FadeAmount");

        protected CancellationTokenSource Cts;
        private Tween bodyProgressTween;
        private GameObject BodyDecalObject => bodyProjector.gameObject;

        public override void Initialize(IProjectionProvider provider)
        {
            Provider = provider;
            
            projector.material     = new Material(materialReference);
            bodyProjector.material = new Material(bodyMaterial);

            UpdateHeadProjector();
            UpdateBodyProjector();
            
            var builder = new CombatSequenceBuilder(provider.Sequence);
            
            builder
                .Add(Section.Active, "ShaderProgression", PlayProjection)
                .Add(Section.Cancel, "CancelTween", StopProjection)
                .Add(Section.Execute, "CancelTween", StopProjection)
                .Add(Section.End, "ResetMaterial", StopProjection);
            
            StopProjection();
        }
        
        protected override void Dispose()
        {
            base.Dispose();
            
            Cts?.Cancel();
            Cts = null;
        }
        

        protected override void PlayProjection()
        {
            DecalObject.SetActive(true);
            BodyDecalObject.SetActive(true);

            PlayProjector(Provider.CastingWeight).Forget();
        }

        protected override void StopProjection()
        {
            projector.material.SetFloat(FillProgressShaderID, 0f);
            bodyProjector.material.SetFloat(FillProgressShaderID, 0f);
            
            Cts?.Cancel();
            Cts = null;
            
            DecalObject.SetActive(false);
            BodyDecalObject.SetActive(false);
        }
        
        protected virtual async UniTaskVoid PlayProjector(float duration)
        {
            Cts = new CancellationTokenSource();
            
            var timer = 0f;

            while (timer < duration)
            {
                timer += Time.deltaTime * 1.0f / duration;
                
                projector.material.SetFloat(FillProgressShaderID, Mathf.Clamp01(-1f + timer * 2));
                bodyProjector.material.SetFloat(FillProgressShaderID, Mathf.Clamp01(timer * 2));
                
                await UniTask.Yield(Cts.Token);
            }
        }
        
        /// <summary>
        /// Updates the head projector properties.
        /// </summary>
        private void UpdateHeadProjector()
        {
            var currentSize = projector.size;
            
            currentSize.x = length + 1 + (width - 1);
            currentSize.y = length + 1;
            currentSize.z = ProjectorDepth;
            
            projector.pivot = new Vector3(0, 0,ProjectorDepth / 2);
            projector.size  = currentSize;
            projector.transform.localPosition =
                new Vector3(0, YPosition, (length + 1) * 0.5f * ArrowZDisplacement);

            projector.material.SetColor(ColorShaderID, backgroundColor);
            projector.material.SetColor(FillColorShaderID, fillColor);
            projector.material.SetFloat(FillProgressShaderID, 0f);
            projector.material.SetFloat(FadeAmountShaderID, fadeAmount * 2 - 0.5f);
        }

        /// <summary>
        /// Updates the body projector properties.
        /// </summary>
        private void UpdateBodyProjector()
        {
            var currentSize = bodyProjector.size;
            
            currentSize.x = length + 1 + (width - 1);
            currentSize.y = length + 1;
            currentSize.z = ProjectorDepth;
            
            bodyProjector.pivot = new Vector3(0, 0,ProjectorDepth / 2);
            bodyProjector.size  = currentSize;
            bodyProjector.transform.localPosition = 
                new Vector3(0, YPosition, (length + 1) * 0.5f);

            bodyProjector.material.SetColor(ColorShaderID, backgroundColor);
            bodyProjector.material.SetColor(FillColorShaderID, fillColor);
            bodyProjector.material.SetFloat(FillProgressShaderID, 0f);
            bodyProjector.material.SetFloat(FadeAmountShaderID, fadeAmount * 2);
        }

        private void OnValidate()
        {
            UpdateHeadProjector();
            UpdateBodyProjector();
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
