// using System;
// using Core;
// using DG.Tweening;
// using UnityEngine;
// using UnityEngine.Rendering.Universal;
//
// namespace Character.Combat.Projector.Decal
// {
//     public class ProjectorDecal : MonoBehaviour, IInspectorSetUp
//     {
//         [SerializeField] private ProjectorObject po; 
//         [SerializeField] private DecalProjector decal;
//
//         private ProjectorShapeType shapeType;
//         private float castingTime;
//         private Vector2 size;
//         private int instanceID;
//
//         public int InstanceID =>
//             instanceID == 0
//                 ? instanceID = GetInstanceID()
//                 : instanceID;
//         
//         private static readonly int FillAmount = Shader.PropertyToID("_FillAmount");
//         private const float Depth = 50f;
//         
//
//         private void StartProjection()
//         {
//             castingTime = po.CastingTime;
//             
//             switch (shapeType)
//             {
//                 case ProjectorShapeType.Sphere:
//                 {
//                     var length = size.x * 2f;
//                     decal.size = new Vector3(length, length, Depth);
//                     
//                     break;
//                 }
//                 case ProjectorShapeType.Rectangle:
//                 {
//                     var providerPosition = po.Provider.Object.transform.position;
//                     var takerPosition = po.Taker != null
//                         ? po.Taker.Object.transform.position
//                         : po.Destination;
//                     var length = Vector3.Distance(providerPosition, takerPosition);
//                     decal.size = new Vector3(length, size.x, Depth);
//
//                     break;
//                 }
//                 case ProjectorShapeType.ExRectangle:
//                 {
//                     decal.size = new Vector3(size.x, 100f, Depth);
//                     break;
//                 }
//                 case ProjectorShapeType.Cone:
//                 {
//                     break;
//                 }
//                 
//                 default: throw new ArgumentOutOfRangeException();
//             }
//             
//             decal.material.SetFloat(FillAmount, 0f);
//             decal.material
//                 .DOFloat(1.5f, FillAmount, castingTime)
//                 .SetEase(Ease.InQuad)
//                 .OnComplete(po.Complete);
//         }
//
//         private void EndProjection()
//         {
//             decal.material.SetFloat(FillAmount, 0f);
//         }
//
//         private void Awake()
//         {
//             po            ??= GetComponentInParent<ProjectorObject>();
//             decal         ??= GetComponent<DecalProjector>();
//             shapeType     =   po.ShapeType;
//             size          =   po.SizeValue;
//             
//             var mat = new Material(decal.material);
//             decal.material = mat;
//
//             po.OnActivated.Register(InstanceID, StartProjection);
//             po.OnCompleted.Register(InstanceID, EndProjection);
//         }
//
// #if UNITY_EDITOR
//         public void SetUp()
//         {
//             po            ??= GetComponentInParent<ProjectorObject>();
//             decal         ??= GetComponent<DecalProjector>();
//             // decalMaterial =   decal.material;
//         }
// #endif
//     }
// }
