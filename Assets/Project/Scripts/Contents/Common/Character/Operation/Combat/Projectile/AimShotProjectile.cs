using UnityEngine;
using DG.Tweening;

namespace Common.Character.Operation.Combat.Projectile
{
    public class AimShotProjectile : ProjectileBehaviour
    {
        protected override void Trajectory()
        {
            TrajectoryTweener = transform
                                .DOMove(Destination, speed)
                                .SetEase(Ease.Linear)
                                .OnComplete(Arrived)
                                .SetSpeedBased();
            
            TrajectoryTweener.OnUpdate(() =>
            {
                var takerPosition = Taker.Object.transform.position;
                
                if (Vector3.Distance(transform.position, takerPosition) > 1f)
                {
                    TrajectoryTweener.ChangeEndValue(takerPosition, speed, true)
                                     .SetSpeedBased();
                }
            });
        }
        
        private void Arrived()
        {
            if (ValidateTaker) Taker.TakeDamage(DamageEntity);
        }
    }
}
