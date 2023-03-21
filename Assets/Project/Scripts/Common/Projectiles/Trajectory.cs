using System;
using DG.Tweening;
using UnityEngine;

namespace Common.Projectiles
{
    public class Trajectory : MonoBehaviour
    {
        private enum TrajectoryType { None, Instant, Straight, Parabola }

        [SerializeField] private TrajectoryType trajectoryType;
        [SerializeField] private float speed;
        [SerializeField] private float distance = 8f;
        [SerializeField] private Ease tweenType;

        private ProjectileComponent projectileComponent;
        private Tween trajectoryTween;
        private Transform projectile;
        private Vector3 Direction => projectile.forward;

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
            projectile.SetPositionAndRotation(Direction, Quaternion.LookRotation(Direction - projectile.position));
        }
        
        private void Straight()
        {
            var projectilePosition = projectile.position;
            var destination        = projectilePosition + Direction * distance;
            var duration           = Vector3.Distance(projectilePosition, destination) / speed;

            trajectoryTween = projectile
                              .DOMove(destination, duration)
                              .SetEase(tweenType)
                              .OnUpdate(() => projectile.LookAt(destination))
                              .OnComplete(() => projectileComponent.End());
        }

        private void Parabola()
        {
            
        }

        private void Stop()
        {
            trajectoryTween?.Kill();
        }

        private void Awake()
        {
            TryGetComponent(out projectileComponent);

            projectile  = projectileComponent.transform;
            
            projectileComponent.OnActivated.Register("Trajectory", Flying);
            projectileComponent.OnCanceled.Register("TrajectoryCancel", Stop);
            // projectileComponent.OnCompleted.Register("TrajectoryCancel", Stop);
        }
    }
}
