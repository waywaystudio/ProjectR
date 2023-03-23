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

        protected const float ProjectorDepth = 50f;
        protected static readonly int FillAmount = Shader.PropertyToID("_FillAmount");
        protected IProjectorSequence ProjectorSequencer;
        protected Func<float> CastingTime;
        protected Func<Vector2> SizeReference;

        protected GameObject DecalObject => projector.gameObject;

        protected abstract void Initialize();

        protected void Activate()
        {
            DecalObject.SetActive(true);

            projector.material.DOFloat(1.5f, FillAmount, CastingTime.Invoke());
        }
        
        protected void Cancel()
        {
            ResetMaterial();
        }
        
        protected void Complete()
        {
            ResetMaterial();
        }
        
        protected void End()
        {
            DecalObject.SetActive(false);
        }


        private void ResetMaterial() => projector.material.SetFloat(FillAmount, 0f);
        
        private void Awake()
        {
            projector.material = new Material(materialReference);
            
            if (!TryGetComponent(out ProjectorSequencer))
            {
                Debug.LogError($"Require IProjectorSequence. Call:{gameObject.name}");
                return;
            }

            CastingTime   += () => ProjectorSequencer.CastingTime;
            SizeReference += () => ProjectorSequencer.SizeVector;

            ProjectorSequencer.OnActivated.Register("Projector", Activate);
            ProjectorSequencer.OnCanceled.Register("Projector", Cancel);
            ProjectorSequencer.OnCompleted.Register("Projector", Complete);
            ProjectorSequencer.OnEnded.Register("Projector", End);

            Initialize();
        }


#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            projector = GetComponentInChildren<DecalProjector>();
        }
#endif
    }
}
