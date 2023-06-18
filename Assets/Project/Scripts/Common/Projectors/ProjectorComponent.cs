using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Common.Projectors
{
    public abstract class ProjectorComponent : MonoBehaviour, IEditable
    {
        [SerializeField] protected Material materialReference;
        [SerializeField] protected DecalProjector projector;
        [SerializeField] protected Ease easeType = Ease.Linear;

        protected const float ProjectorDepth = 50f;
        protected static readonly int ProgressID = Shader.PropertyToID("_Progress");
        protected Func<float> CastingTime;
        protected Func<Vector2> SizeReference;

        protected GameObject DecalObject => projector.gameObject;

        protected virtual void Initialize() { }
        protected virtual void OnObject() => DecalObject.SetActive(true);
        protected virtual void OffObject() => DecalObject.SetActive(false);
        
        protected void ResetMaterial() => projector.material.SetFloat(ProgressID, 0f);
        protected void PlayShader() 
            => projector.material.DOFloat(1.0f, ProgressID, CastingTime.Invoke()).SetEase(easeType);
        

        private void Awake()
        {
            projector.material = new Material(materialReference);

            var sequencer = GetComponentInParent<IProjectorSequencer>();
            var builder = new SequenceBuilder(sequencer);

            if (sequencer is null) return;
            
            // Require CastTime & SizeReference
            CastingTime   += () => sequencer.CastWeightTime;
            SizeReference += () => sequencer.SizeVector;

            builder.Add(SectionType.Active, "ActiveDecalObject", OnObject)
                   .Add(SectionType.Active, "ShaderProgression", PlayShader)
                   .Add(SectionType.End, "ResetMaterial", ResetMaterial)
                   .Add(SectionType.End, "DeActiveDecalObject", OffObject);
            
            Initialize();
            ResetMaterial();
            OffObject();
        }


#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            projector = GetComponentInChildren<DecalProjector>();
        }
#endif
    }
}
