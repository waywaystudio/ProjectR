using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Projectiles
{
    [Serializable]
    public class Trajectory
    {
        private enum TrajectoryType { None, Instant, Straight, Parabola }

        [SerializeField] private TrajectoryType trajectoryType;
        [SerializeField] private float speed = 10f;
        [SerializeField] private float distance = 35f;

        private CancellationTokenSource cts;
        private Projectile projectile;
        private Vector3 Direction => projectile.transform.forward;


        public void Initialize(Projectile projectile)
        {
            this.projectile = projectile;
            this.projectile.Builder
                .Add(Section.Active, "Trajectory", Flying)
                .Add(Section.End, "Trajectory", StopStraight);
        }

        public void Flying()
        {
            switch (trajectoryType)
            {
                case TrajectoryType.Instant:  { Instant(); break; }
                case TrajectoryType.Straight: { Straight().Forget(); break; }
                case TrajectoryType.Parabola: { Parabola(); break; }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Dispose()
        {
            StopStraight();
        }
        

        private void Instant()
        {
            var projectilePosition = projectile.transform.position;
            var destination        = projectilePosition + Direction * distance;
            
            projectile.transform
                               .SetPositionAndRotation(destination, Quaternion.LookRotation(Direction - projectilePosition));
        }

        private async UniTaskVoid Straight()
        {
            cts = new CancellationTokenSource();
            
            var departLength = 0f;
            var travelDistancePerFrame = Direction * (speed * Time.deltaTime);

            while (departLength < distance)
            {
                var transform = projectile.transform;
                var position = transform.position;

                position           += travelDistancePerFrame;
                transform.position =  position;

                departLength += travelDistancePerFrame.magnitude;

                await UniTask.Yield(cts.Token);
            }

            projectile.Invoker.Complete();
        } 

        private void Parabola()
        {
            
        }

        private void StopStraight()
        {
            cts?.Cancel();
            cts = null;
        }
    }
}