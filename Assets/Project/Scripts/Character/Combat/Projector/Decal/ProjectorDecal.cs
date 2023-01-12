using System;
using Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Character.Combat.Projector.Decal
{
    public class ProjectorDecal : MonoBehaviour, IEditorSetUp
    {
        [SerializeField] private ProjectorObject po; 
        [SerializeField] private DecalProjector decal;
        [SerializeField] private Material decalMaterial;

        private ProjectorShapeType shapeType;
        private float castingTime;
        private Vector2 size;
        private int instanceID;

        public int InstanceID =>
            instanceID == 0
                ? instanceID = GetInstanceID()
                : instanceID;
        
        private static readonly int FillAmount = Shader.PropertyToID("_FillAmount");
        private const float Depth = 50f;


        private void StartProjection()
        {
            switch (shapeType)
            {
                case ProjectorShapeType.Sphere:
                {
                    var length = size.x * 2f;
                    decal.size = new Vector3(length, length, Depth);
                    break;
                }
                case ProjectorShapeType.Rectangle:
                {
                    break;
                }
                case ProjectorShapeType.ExtendedRectangle:
                {
                    break;
                }
                case ProjectorShapeType.Cone:
                {
                    break;
                }
                
                default: throw new ArgumentOutOfRangeException();
            }
            
            decalMaterial.SetFloat(FillAmount, 0f);
            decalMaterial
                .DOFloat(1.5f, FillAmount, castingTime)
                .SetEase(Ease.InQuad)
                .OnComplete(po.OnProjectionEnd.Invoke);
        }

        private void EndProjection()
        {
            decalMaterial.SetFloat(FillAmount, 0f);
        }

        private void Awake()
        {
            po            ??= GetComponentInParent<ProjectorObject>();
            decal         ??= GetComponent<DecalProjector>();
            decalMaterial =   decal.material;
            shapeType     =   po.ShapeType;
            castingTime   =   po.CastingTime;
            size          =   po.SizeValue;

            po.OnProjectionStart.Register(InstanceID, StartProjection);
            po.OnProjectionEnd.Register(InstanceID, EndProjection);
        }

#if UNITY_EDITOR
        public void SetUp()
        {
            po            ??= GetComponentInParent<ProjectorObject>();
            decal         ??= GetComponent<DecalProjector>();
            decalMaterial =   decal.material;
        }
#endif
    }
}
