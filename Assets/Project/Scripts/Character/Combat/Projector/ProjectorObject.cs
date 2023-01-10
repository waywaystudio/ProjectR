using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Character.Combat.Projector
{
    public abstract class ProjectorObject : MonoBehaviour
    {
        [SerializeField] protected ProjectorCollider projectorCollider;
        [SerializeField] protected DecalProjector decal;
        [SerializeField] protected Material decalMaterial;
        [SerializeField] protected LayerMask targetLayer;
        [SerializeField] protected float modifier;
        // 1. Radius at SphereArea;
        // 2. Width at RectangleArea;
        // 3. Angle at ConeArea;
        

        public abstract void Generate(Vector3 pointA, Vector3 pointB);

        protected abstract void Play();
        // protected abstract void Stop();
        
        protected virtual void Awake()
        {
            var length = modifier * 2f;
            
            projectorCollider ??= GetComponentInChildren<ProjectorCollider>(); 
            decal             ??= GetComponentInChildren<DecalProjector>();
            decalMaterial     ??= decal.material;
            decal.size        =   new Vector3(length, length, 50f);
            targetLayer       =   LayerMask.GetMask("Adventurer");

            projectorCollider.Initialize(targetLayer, modifier);
        }


#if UNITY_EDITOR
        public virtual void SetUp()
        {
            var length = modifier * 2f;
            
            projectorCollider ??= GetComponentInChildren<ProjectorCollider>(); 
            decal             ??= GetComponentInChildren<DecalProjector>();
            decalMaterial     ??= decal.material;
            decal.size        =   new Vector3(length, length, 50f);
            targetLayer       =   LayerMask.GetMask("Adventurer");
        }
#endif
    }
}
