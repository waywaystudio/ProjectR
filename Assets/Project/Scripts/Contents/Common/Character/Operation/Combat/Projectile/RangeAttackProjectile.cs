using DG.Tweening;
using UnityEngine;

namespace Common.Character.Operation.Combat.Projectile
{
    public class RangeAttackProjectile : ProjectileBehaviour
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
        
        protected void Arrived()
        {
            if (ValidateTaker) Taker.TakeDamage(DamageEntity);
            
            // TODO. Return To Pool
            Destroy(gameObject);
        }
    }
}
