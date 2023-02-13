// using DG.Tweening;
// using UnityEngine;
//
// namespace Character.Combat.Projectile
// {
//     public class FireballProjectile : ProjectileObject
//     {
//         protected override void Trajectory()
//         {
//             TrajectoryTweener = transform
//                                 .DOMove(Destination, speed)
//                                 .SetEase(Ease.Linear)
//                                 .OnComplete(Complete)
//                                 .SetSpeedBased();
//             
//             TrajectoryTweener.OnUpdate(() =>
//             {
//                 var takerPosition = Taker.Object.transform.position;
//                 
//                 if (Vector3.Distance(transform.position, takerPosition) > 1f)
//                 {
//                     TrajectoryTweener.ChangeEndValue(takerPosition, speed, true)
//                                      .SetSpeedBased();
//                 }
//             });
//         }
//         
//         protected override void Arrived()
//         {
//             if (ValidateTaker)
//             {
//                 Taker.TakeDamage(DamageModule);
//                 StatusEffectModule.Effectuate(Taker);
//             }
//         }
//     }
// }
