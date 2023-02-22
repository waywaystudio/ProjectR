// using Core;
// using UnityEngine;
//
// namespace Character.Combat.Projector.Colliders
// {
//     public class RectProjectorCollider : ProjectorCollider
//     {
//         [SerializeField] private BoxCollider boxCollider;
//         
//         private const float BoxHeight = 2f;
//         private Vector3 halfExtendBuffer;
//         private Quaternion orientationBuffer;
//
//
//         protected override void StartProjection()
//         {
//             var providerPosition = po.Provider.Object.transform.position;
//             var takerPosition = po.Taker != null
//                 ? po.Taker.Object.transform.position
//                 : po.Destination;
//             
//             TargetLayer           = po.TargetLayer;
//             po.transform.position = (providerPosition + takerPosition) * 0.5f;
//             boxCollider.size      = GenerateDynamicSize(providerPosition, takerPosition);
//
//             LookAt(takerPosition, out orientationBuffer);
//         }
//
//         protected override void EndProjection()
//         {
//             Physics.OverlapBoxNonAlloc(transform.position, halfExtendBuffer, ColliderBuffer, orientationBuffer, TargetLayer);
//
//             if (ColliderBuffer.IsNullOrEmpty()) return;
//             
//             ColliderBuffer.ForEach(x =>
//             {
//                 if (x.IsNullOrEmpty()) return;
//                 if (!x.TryGetComponent(out ICombatTaker taker)) return;
//                 
//                 po.OnProjectorEnd.Invoke(taker);
//             });
//         }
//
//         private Vector3 GenerateDynamicSize(Vector3 providerPosition, Vector3 takerPosition)
//         {
//             var result = new Vector3(Vector3.Distance(providerPosition, takerPosition),BoxHeight, SizeValue.x );
//
//             halfExtendBuffer = result * 0.5f;
//
//             return result;
//         }
//
//         private void LookAt(Vector3 takerPosition, out Quaternion buffer)
//         {
//             var poTransform = po.transform;
//             poTransform.LookAt(takerPosition);
//
//             buffer = poTransform.rotation;
//         }
//         
//         
//         protected override void Awake()
//         {
//             base.Awake();
//
//             boxCollider ??= GetComponent<BoxCollider>();
//         }
//
// #if UNITY_EDITOR
//         public override void SetUp()
//         {
//             base.SetUp();
//             
//             boxCollider = GetComponent<BoxCollider>();
//         }
// #endif
//     }
// }
