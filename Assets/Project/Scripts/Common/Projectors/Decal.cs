using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Common.Projectors
{
    public class Decal : MonoBehaviour, IAssociate<Projection>
    {
        [SerializeField] protected Material materialReference;
        [SerializeField] protected Color backgroundColor = Color.white;
        [SerializeField] protected Color fillColor = Color.red;

        #region ShaderPropertyID
        protected const float ProjectorDepth = 50f;
        protected static readonly int ColorShaderID = Shader.PropertyToID("_Color");
        protected static readonly int FillColorShaderID = Shader.PropertyToID("_FillColor");
        protected static readonly int FillProgressShaderID = Shader.PropertyToID("_FillProgress");
        protected static readonly int ArcShaderID = Shader.PropertyToID("_Arc");
        protected static readonly int AngleShaderID = Shader.PropertyToID("_Angle");
        protected static readonly int InnerRadiusID = Shader.PropertyToID("_InnerRadius");
        #endregion

        protected DecalProjector DecalProjector;
        protected CancellationTokenSource Cts;
        protected SizeEntity Size;
        protected IProjection Provider { get; set; }
        protected TargetingType TargetingType;
        protected float ArcAngleNormalizer;
        protected Vector3 ProjectorSize
        {
            get
            {
                var defaultSize = new Vector3(DecalProjector.size.x, DecalProjector.size.y, ProjectorDepth);

                defaultSize = TargetingType switch
                {
                    TargetingType.Circle => new Vector3(Size.AreaRange * 2f, Size.AreaRange * 2f, ProjectorDepth),
                    TargetingType.Arc    => new Vector3(Size.AreaRange * 2f, Size.AreaRange * 2f, ProjectorDepth),
                    TargetingType.Rect   => new Vector3(Size.Width, Size.Height, ProjectorDepth),
                    TargetingType.Donut  => new Vector3(Size.OuterRadius * 2f, Size.OuterRadius * 2f, ProjectorDepth),
                    _                    => defaultSize
                };

                return defaultSize;
            }
        }
        

        public virtual void Initialize(Projection master)
        {
            if (!Verify.IsTrue(TryGetComponent(out DecalProjector))) return;
            
            Provider           = master.Provider;
            TargetingType      = master.TargetingType;
            Size               = master.Provider.SizeEntity;
            ArcAngleNormalizer = Mathf.Clamp(1f - Size.Angle / 360, 0f, 360f);

            DecalProjector.material = new Material(materialReference);
            DecalProjector.size     = ProjectorSize;
            
            DecalProjector.material.SetFloat(FillProgressShaderID, 0f);
            DecalProjector.material.SetFloat(ArcShaderID, ArcAngleNormalizer);
            DecalProjector.material.SetColor(ColorShaderID, backgroundColor);
            DecalProjector.material.SetColor(FillColorShaderID, fillColor);
            DecalProjector.material.SetFloat(AngleShaderID, 0f);
            DecalProjector.material.SetFloat(InnerRadiusID, Size.InnerRadius / Size.OuterRadius);
            
            master.Builder
                  .Add(Section.Active, $"{master.InstanceKey}.PlayProjection", () => PlayProjection().Forget())
                  .Add(Section.Cancel, $"{master.InstanceKey}.CancelTween", StopProjection)
                  .Add(Section.Execute, $"{master.InstanceKey}.CancelTween", StopProjection)
                  .Add(Section.End, $"{master.InstanceKey}.ResetMaterial", StopProjection);

            StopProjection();
        }


        protected virtual async UniTaskVoid PlayProjection()
        {
            gameObject.SetActive(true);
            DecalProjector.material.SetFloat(FillProgressShaderID, 0f);

            Cts = new CancellationTokenSource();

            var timer = 0f;
            var inverseDuration = 1.0f / Provider.CastingTime;

            while (timer < Provider.CastingTime)
            {
                timer += Time.deltaTime * inverseDuration;
                timer =  Mathf.Clamp01(timer);
                
                DecalProjector.material.SetFloat(FillProgressShaderID, timer);

                await UniTask.Yield(Cts.Token);
            }
        }

        protected virtual void StopProjection()
        {
            Cts?.Cancel();
            Cts = null;
            
            DecalProjector.material.SetFloat(FillProgressShaderID, 0f);
            gameObject.SetActive(false);
        }

        protected void OnDestroy()
        {
            Cts?.Cancel();
            Cts = null;
        }


#if UNITY_EDITOR
        private float FillProgress { get; set; }
        private float Arc { get; set; }
        private float Angle { get; set; }
        private float InnerRadius { get; set; }

        private void Simulate()
        {
            // DecalProjector.material ??= new Material(materialReference);
            // DecalProjector.size     =   ProjectorSize;
            //
            // DecalProjector.material.SetFloat(FillProgressShaderID, 0f);
            // DecalProjector.material.SetFloat(ArcShaderID, ArcAngleNormalizer);
            // DecalProjector.material.SetColor(ColorShaderID, backgroundColor);
            // DecalProjector.material.SetColor(FillColorShaderID, fillColor);
            // DecalProjector.material.SetFloat(AngleShaderID, 0f);
            // DecalProjector.material.SetFloat(InnerRadiusID, Size.InnerRadius / Size.OuterRadius);
        }

        private void OnValidate()
        {
            if (!Verify.IsTrue(TryGetComponent(out DecalProjector))) return;
            if (materialReference is null) return;
            
            Simulate();
        }
#endif
    }
}
