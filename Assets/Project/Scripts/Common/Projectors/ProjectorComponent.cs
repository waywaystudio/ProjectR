using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Common.Projectors
{
    public abstract class ProjectorComponent : MonoBehaviour, ISequencerHolder, IEditable
    {
        [SerializeField] protected bool isStaticProjector;
        [SerializeField] protected Material materialReference;
        [SerializeField] protected DecalProjector projector;
        [SerializeField] protected Ease easeType = Ease.Linear;
        [SerializeField] protected Color backgroundColor = Color.white;
        [SerializeField] protected Color fillColor = Color.red;

        protected const float ProjectorDepth = 50f;
        protected Tween ProgressTween;

        public Sequencer Sequencer { get; } = new();
        public SequenceBuilder Builder { get; private set; }
        public SequenceInvoker Invoker { get; private set; }

        protected IProjectionProvider Provider { get; set; }
        protected GameObject DecalObject => projector.gameObject;
        protected float ArcAngleNormalized => Mathf.Clamp(1f - Provider.SizeVector.z / 360, 0f, 360f);
        
        protected static readonly int ColorShaderID = Shader.PropertyToID("_Color");
        protected static readonly int FillColorShaderID = Shader.PropertyToID("_FillColor");
        protected static readonly int FillProgressShaderID = Shader.PropertyToID("_FillProgress");
        protected static readonly int ArcShaderID = Shader.PropertyToID("_Arc");
        protected static readonly int AngleShaderID = Shader.PropertyToID("_Angle");



        public void Initialize(IProjectionProvider provider)
        {
            Provider = provider;
            
            // Set Projector Radius, Angle
            var diameter = Provider.SizeVector.y * 2f;
            
            projector.material = new Material(materialReference);
            projector.size     = new Vector3(diameter, diameter, ProjectorDepth);
            
            projector.material.SetFloat(FillProgressShaderID, 0f);
            projector.material.SetFloat(ArcShaderID, ArcAngleNormalized);
            projector.material.SetColor(ColorShaderID, backgroundColor);
            projector.material.SetColor(FillColorShaderID, fillColor);
            projector.material.SetFloat(AngleShaderID, 0f);

            var builder = new CombatSequenceBuilder(provider.Sequence);
            
            builder
                .Add(Section.Active, "ShaderProgression", PlayProjection)
                .Add(Section.Cancel, "CancelTween", StopProjection)
                .Add(Section.Execute, "CancelTween", StopProjection)
                .Add(Section.End, "ResetMaterial", StopProjection);

            if (isStaticProjector)
            {
                ActiveProjector();
            }

            StopProjection();
        }

        public void Dispose()
        {
            Sequencer.Clear();
        }

        public void ActiveProjector()
        {
            Builder.Register("ProjectorSequencer", Provider.Sequence);
        }

        public void DeActiveProjector()
        {
            Invoker.Cancel();
            Builder.Unregister("ProjectorSequencer", Provider.Sequence);
        }
        

        protected virtual void Awake()
        {
            Provider = GetComponentInParent<IProjectionProvider>();

            if (!Verify.IsNotNull(Provider, $"Projector Require IProjectionProvider!")) 
                return;
            
            // Set Projector Radius, Angle
            var diameter = Provider.SizeVector.y * 2f;
            
            projector.material = new Material(materialReference);
            
            // projector.material.SetFloat(FillProgressShaderID, Provider.CastingWeight);
            projector.size = new Vector3(diameter, diameter, ProjectorDepth);
            projector.material.SetFloat(FillProgressShaderID, 0f);
            projector.material.SetFloat(ArcShaderID, ArcAngleNormalized);
            projector.material.SetColor(ColorShaderID, backgroundColor);
            projector.material.SetColor(FillColorShaderID, fillColor);
            projector.material.SetFloat(AngleShaderID, 0f);

            Invoker = new SequenceInvoker(Sequencer);
            Builder = new SequenceBuilder(Sequencer);
            Builder
                .Add(Section.Active, "ShaderProgression", PlayProjection)
                .Add(Section.Cancel, "CancelTween", StopProjection)
                .Add(Section.End, "ResetMaterial", StopProjection);

            if (isStaticProjector)
            {
                ActiveProjector();
            }

            StopProjection();
        }
        
        
        private void PlayProjection()
        {
            DecalObject.SetActive(true);
            projector.material.SetFloat(FillProgressShaderID, 0f);
            ProgressTween = projector.material
                                     .DOFloat(1.0f, FillProgressShaderID, Provider.CastingWeight)
                                     .SetEase(easeType);
        }

        private void StopProjection()
        {
            if (ProgressTween != null)
            {
                ProgressTween.Kill();
                ProgressTween = null;
            }

            projector.material.SetFloat(FillProgressShaderID, 0f);
            DecalObject.SetActive(false);
        }


#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            projector = GetComponentInChildren<DecalProjector>();
        }
#endif
    }
}
