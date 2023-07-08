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
        
        [Sirenix.OdinInspector.ShowInInspector]
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


        public virtual void Dispose()
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
                .Add(Section.Active, "ActiveDecalObject", OnObject)
                .Add(Section.Active, "ShaderProgression", PlayShader)
                .Add(Section.Cancel, "CancelTween", CancelTween)
                .Add(Section.End, "ResetMaterial", ResetMaterial)
                .Add(Section.End, "DeActiveDecalObject", OffObject);

            if (isStaticProjector)
            {
                ActiveProjector();
            }

            OffObject();
        }

        private void OnObject() => DecalObject.SetActive(true);
        private void OffObject() => DecalObject.SetActive(false);
        private void ResetMaterial() => projector.material.SetFloat(FillProgressShaderID, 0f);
        private void PlayShader()
        {
            ProgressTween = projector.material
                                     .DOFloat(1.0f, FillProgressShaderID, Provider.CastingWeight)
                                     .SetEase(easeType);
        }

        private void CancelTween()
        {
            if (ProgressTween == null) return;
            
            ProgressTween.Kill();
            ProgressTween = null;
        }


#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            projector = GetComponentInChildren<DecalProjector>();
        }
#endif
    }
}
