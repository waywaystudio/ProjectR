using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Projectors
{
    public class RectProjector : Projector
    {
        [PropertyRange(0, 1)]
        [SerializeField] protected float fillSimulator;
        [SerializeField] protected float width;
        [SerializeField] protected float length;

        /// <summary>
        /// The projector's 'y' position.
        /// </summary>
        protected const float YPosition = 0.15f;
        protected CancellationTokenSource Cts;
        
        
        public override void Initialize(IProjection provider)
        {
            Provider           = provider;
            projector.material = new Material(materialReference);
            width              = provider.SizeEntity.Width;
            length             = provider.SizeEntity.Height;

            UpdateHeadProjector();

            Builder = new SequenceBuilder(Sequencer);
            Builder.Register($"{InstanceKey}.Projection", Provider.Sequence);
            
            Builder
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

            PlayProjector(Provider.CastingTime).Forget();
        }

        protected override void StopProjection()
        {
            projector.material.SetFloat(FillProgressShaderID, 0f);
            DecalObject.SetActive(false);
            
            Cts?.Cancel();
            Cts = null;
        }
        
        protected async UniTaskVoid PlayProjector(float duration)
        {
            Cts = new CancellationTokenSource();
            
            var timer = 0f;
            var reverseDuration = 1.0f / duration;

            while (timer < duration)
            {
                timer += Time.deltaTime * reverseDuration;
                
                projector.material.SetFloat(FillProgressShaderID, timer);

                await UniTask.Yield(Cts.Token);
            }
        }
        
        /// <summary>
        /// Updates the head projector properties.
        /// </summary>
        private void UpdateHeadProjector()
        {
            var currentSize = projector.size;

            currentSize.x = width;
            currentSize.y = length;
            currentSize.z = ProjectorDepth;
            
            projector.pivot                   = new Vector3(0, 0,ProjectorDepth / 2);
            projector.size                    = currentSize;
            projector.transform.localPosition = new Vector3(0, YPosition, length * 0.5f);

            projector.material.SetColor(ColorShaderID, backgroundColor);
            projector.material.SetColor(FillColorShaderID, fillColor);
            projector.material.SetFloat(FillProgressShaderID, 0f);
        }
        
        private void OnValidate()
        {
            UpdateHeadProjector();
            projector.material.SetFloat(FillProgressShaderID, fillSimulator);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
