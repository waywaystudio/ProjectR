using Core;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Character.Projector
{
    public class ProjectorComponent : MonoBehaviour
    {
        [SerializeField] private Material materialReference;
        [SerializeField] protected DecalProjector projector;

        protected const float ProjectorDepth = 50f;
        protected float CastingTime;
        protected bool IsFirst = true;

        [ShowInInspector] public ActionTable OnActivated { get; } = new();
        [ShowInInspector] public ActionTable OnCompleted { get; } = new();
        [ShowInInspector] public ActionTable OnInterrupted { get; } = new();
        [ShowInInspector] public ActionTable OnEnded { get; } = new();

        protected Material Material => projector.material;
        protected static readonly int FillAmount = Shader.PropertyToID("_FillAmount");


        protected void InitialSetUp()
        {
            projector          ??= GetComponentInChildren<DecalProjector>();
            projector.material =   new Material(materialReference);
            
            OnActivated.Register("Active", () => gameObject.SetActive(true));
            OnActivated.Register("Fill", () => Material.DOFloat(1.5f, FillAmount, CastingTime));
            
            OnCompleted.Register("ResetFill", () => Material.SetFloat(FillAmount, 0f));
            
            OnEnded.Register("DeActive", () => gameObject.SetActive(false));

            IsFirst = false;
        }


        public virtual void SetUp()
        {
            projector ??= GetComponentInChildren<DecalProjector>();
        }
    }
}
