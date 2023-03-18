using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Common.Projectors
{
    public abstract class ProjectorComponent : MonoBehaviour, ISequence, IEditable
    {
        [SerializeField] private Material materialReference;
        [SerializeField] protected DecalProjector projector;

        protected const float ProjectorDepth = 50f;
        protected static readonly int FillAmount = Shader.PropertyToID("_FillAmount");
        protected float CastingTime;

        public ActionTable OnActivated { get; } = new();
        public ActionTable OnCanceled { get; } = new();
        public ActionTable OnCompleted { get; } = new();
        public ActionTable OnEnded { get; } = new();


        protected void CoreInitialize(ISequence mainSequence)
        {
            projector.material = new Material(materialReference);

            OnActivated.Register("Active", () => gameObject.SetActive(true));
            OnActivated.Register("Fill", () => projector.material.DOFloat(1.5f, FillAmount, CastingTime));

            OnCanceled.Register("ResetMaterial", ResetMaterial);
            OnCompleted.Register("ResetMaterial", ResetMaterial);
            OnEnded.Register("DeActive", () => gameObject.SetActive(false));
            
            mainSequence.Combine("Projector", this);
        }

        public void Dispose()
        {
            this.Clear();
        }


        private void ResetMaterial() => projector.material.SetFloat(FillAmount, 0f);


#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            projector = GetComponentInChildren<DecalProjector>();
        }
#endif
    }
}
