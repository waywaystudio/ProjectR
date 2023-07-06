using System;
using DG.Tweening;
using UnityEngine;

namespace Common.Projectiles
{
    [Serializable]
    public class Trajectory
    {
        private enum TrajectoryType { None, Instant, Straight, Parabola }

        [SerializeField] private TrajectoryType trajectoryType;
        [SerializeField] private float speed;
        [SerializeField] private float distance = 8f;
        [SerializeField] private Ease tweenType;

        private ProjectileComponent projectileComponent;
        private Tween trajectoryTween;
        private Vector3 Direction => projectileComponent.transform.forward;


        public void Initialize(ProjectileComponent projectile)
        {
            projectileComponent = projectile;
            projectileComponent.Builder
                               .Add(SectionType.Active, "Trajectory", Flying)
                               .Add(SectionType.End, "Trajectory", Stop);
        }

        public void Flying()
        {
            switch (trajectoryType)
            {
                case TrajectoryType.Instant:  { Instant(); break; }
                case TrajectoryType.Straight: { Straight(); break; }
                case TrajectoryType.Parabola: { Parabola(); break; }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        

        private void Instant()
        {
            var projectilePosition = projectileComponent.transform.position;
            var destination        = projectilePosition + Direction * distance;
            
            projectileComponent.transform
                               .SetPositionAndRotation(destination, Quaternion.LookRotation(Direction - projectilePosition));
        }
        
        private void Straight()
        {
            var projectilePosition = projectileComponent.transform.position;
            var destination        = projectilePosition + Direction * distance;
            var duration           = Vector3.Distance(projectilePosition, destination) / speed;

            trajectoryTween = projectileComponent.transform
                                                 .DOMove(destination, duration)
                                                 .SetEase(tweenType)
                                                 .OnUpdate(() => projectileComponent.transform.LookAt(destination))
                                                 .OnComplete(() => projectileComponent.Invoker.End());
        }

        private void Parabola()
        {
            
        }

        private void Stop()
        {
            trajectoryTween?.Kill();
        }
    }
}
