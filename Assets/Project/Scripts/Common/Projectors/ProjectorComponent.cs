using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Common.Projectors
{
    public abstract class ProjectorComponent : MonoBehaviour, ISequence, IEditable
    {
        [SerializeField] protected Material materialReference;
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
            
            mainSequence.CombineAsReference("Projector", this);
        }
        
        private void ResetMaterial() => projector.material.SetFloat(FillAmount, 0f);
        
        public void Dispose()
        {
            this.Clear();
        }

        // TODO. Projector를 독립시킬 준비가 되면 구현해보자.
        // protected ISequence MainSequence;
        
        // protected void Activate()
        // {
        //     gameObject.SetActive(true);
        //     projector.material.DOFloat(1.5f, FillAmount, CastingTime);
        // }
        //
        // protected void Cancel()
        // {
        //     ResetMaterial();
        // }
        //
        // protected void Complete()
        // {
        //     ResetMaterial();
        // }
        //
        // protected void End()
        // {
        //     gameObject.SetActive(false);
        // }

        // private void Awake()
        // {
        //     /*
        //      * Projector는 Skill(ex.MoraggSpin) 혹은 StatusEffect(ex.MoraggLivingBomb) 에 붙는다.             
        //      * Projector를 완전히 분리시키려면, FillTIme(=CastingTime), 과 Radius 개념이 필요하다.
        //      */
        //     
        //     if (!TryGetComponent(out MainSequence))
        //     {
        //         Debug.LogError($"Require ISequence. Call:{gameObject.name}");
        //         return;
        //     }
        //     
        //     projector.material = new Material(materialReference);
        //     
        //     MainSequence.OnActivated.Register("Projector", Activate);
        //     MainSequence.OnCanceled.Register("Projector", Cancel);
        //     MainSequence.OnCompleted.Register("Projector", Complete);
        //     MainSequence.OnEnded.Register("Projector", End);
        // }


#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            projector = GetComponentInChildren<DecalProjector>();
        }
#endif
    }
}
