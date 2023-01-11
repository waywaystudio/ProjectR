using Core;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Character.Combat.Projector
{
    public abstract class ProjectorObject : MonoBehaviour
    {
        [SerializeField] protected ProjectorShapeType shapeType;
        [SerializeField] protected DecalProjector projectorDecal;
        [SerializeField] protected Material decalMaterial;
        [SerializeField] protected ProjectorEvent projectorEvent; 
        [SerializeField] protected LayerMask targetLayer;
        [SerializeField] protected float sizeValue;
        // 1. SphereType : Radius
        // 2. RectangleType : Width
        // 3. ExpandedRectangleType : Width
        // 4. ConeType : Angle

        protected static readonly int FillAmount = Shader.PropertyToID("_FillAmount");
        protected static readonly int CommonFinishAction = "OnCommonFinishAction".GetHashCode();

        protected ICombatProvider Provider;
        protected ICombatTaker Taker;

        protected abstract Collider ProjectorCollider { get; set; }
        protected ActionTable OnFinished { get; } = new();
        protected float CastingTime { get; private set; }

        public void Generate(ICombatProvider provider, ICombatTaker taker, float castingTime)
        {
            Provider    = provider;
            Taker       = taker;
            CastingTime = castingTime;
            
            projectorEvent.Initialize(Provider, Taker, sizeValue, targetLayer);
            
            // Set PrefabTransform
            SetTransform();
            
            // Set Decal & Collider Size
            OnGenerated();
        }

        protected abstract void SetTransform();
        protected abstract void OnGenerated();

        private void OnCommonFinishAction()
        {
            decalMaterial.SetFloat(FillAmount, 0f);
            ProjectorCollider.enabled = false;
            // return to pool;
        }

        protected virtual void Awake()
        {
            projectorDecal ??= GetComponentInChildren<DecalProjector>();
            projectorEvent ??= GetComponent<ProjectorEvent>();
            decalMaterial  =   projectorDecal.material;

            OnFinished.Register(CommonFinishAction, OnCommonFinishAction);
        }

#if UNITY_EDITOR
        public void SetUp()
        {
            projectorDecal    ??= GetComponentInChildren<DecalProjector>();
            projectorEvent    ??= GetComponent<ProjectorEvent>();
            ProjectorCollider ??= GetComponent<Collider>();
            decalMaterial     =   projectorDecal.material;
            
            targetLayer   =   LayerMask.GetMask("Adventurer");
        }
#endif
    }
}
