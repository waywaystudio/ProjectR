using Core;
using UnityEngine;

namespace Common.Projectile
{
    public class HealOrbBehaviour : ProjectileBehaviour
    {
        public override Vector3 Destination
        {
            get
            {
                if (Taker != null && !Taker.Object.IsNullOrEmpty() && Taker.IsAlive)
                {
                    var takerPosition = Taker.Object.transform.position;
                    var direction = (takerPosition - transform.position).normalized;
                    var offset = direction * 5f;
                    
                    destination = takerPosition + offset;
                }

                return destination;
            }
            set => destination = value;
        }
        
        public override void Trajectory()
        {
            // TrajectoryTweener = transform
            //     .DOMove(Destination, speed)
            //     .SetEase(Ease.OutCubic)
            //     .SetLoops(2, LoopType.Yoyo)
            //     .SetSpeedBased();
        }
    }
}
