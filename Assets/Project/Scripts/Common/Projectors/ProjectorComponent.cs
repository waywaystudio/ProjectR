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
        protected IProjectorSequence ProjectorSequencer;
        protected Func<float> CastingTime;
        protected Func<Vector2> SizeReference;

        protected GameObject DecalObject => projector.gameObject;

        protected abstract void Initialize();

        protected void Activate()
        {
            OnObject();
            projector.material.DOFloat(1.0f, ProgressID, CastingTime.Invoke())
                     .SetEase(easeType);
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
            OffObject();
        }


        protected virtual void OnObject()
        {
            DecalObject.SetActive(true);
        }

        protected virtual void OffObject()
        {
            DecalObject.SetActive(false);
        }
        
        protected void ResetMaterial() => projector.material.SetFloat(ProgressID, 0f);

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

            ProjectorSequencer.ActiveAction.Register("Projector", Activate);
            ProjectorSequencer.CancelAction.Register("Projector", Cancel);
            ProjectorSequencer.CompleteAction.Register("Projector", Complete);
            ProjectorSequencer.EndAction.Register("Projector", End);

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
